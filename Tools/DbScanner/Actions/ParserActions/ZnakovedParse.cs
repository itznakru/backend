using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using DbScanner.Exceptions;
using DbScanner.Process.Infrastruction;
using ItZnak.Infrastruction.Extentions;
using MongoDB.Bson;
using PatentService.Types;

namespace DbScanner.Actions.ParserActions
{
    public class ZnakovedSiteParser : ISiteParser
    {
        const string DATE_FORMAT = "dd.MM.yyyy";
        const string CULT_PROVIDER_NAME = "ru-Ru";
        const string URL_TEMPLATE_REFER = "https://www.znakoved.ru/wtms/{0}/";
        const string URL_TEMPLATE_DOWNLOAD = "https://www.znakoved.ru/wtms.php?action=details&uid={0}";

        const string URL_IMAGE = "https://www.znakoved.ru/wtms.php?action=fullsize&RegId={0}";

        public async Task<TradeMark> ProcessAsync(int docId, List<Tuple<string, string>> proxyList)
        {
            /* get html */
            string html = await GetHtmlAsync(docId);
            string image = await GetImage(docId);

            CheckHtml(html);
            TradeMark tm = ParseHtml(html, image, docId);
            return tm;
        }

        /* check key fields of HTML and if it's not normal HTML throw exception */
        private static string CheckHtml(string html)
        {
            return html;
        }
        private static TradeMark ParseHtml(string html, string image, int docId) => new()
        {
            TMPhrase = GetPhrase(html),
            TMNumber = GetNumber(html, docId),
            DocId = GetNumber(html, docId),
            Address = GetAdress(html, docId),
            TMOwner = GetOwner(html, docId),
            MktuList = GetMktuList(html, docId),
            RequestNumber = GetRequestNumber(html, docId),
            RequestDate = GetRequestDate(html, docId),// дата заявки
            TMDate = GetTmDate(html), // дата регистрации
            TMFinishDate = GetTmFinishDate(html, docId), // дата окончания регистрации
            TMPriorityDate = GetTmPriorityDate(html), // дата приоритета
            Image = image
        };

        private static async Task<string> GetImage(int docId)
        {
            using var http = new HttpClient();
            var response = await http.GetAsync(string.Format(URL_IMAGE, docId));
            var image = await response.Content.ReadAsByteArrayAsync();
            return Convert.ToBase64String(image);
        }

        private static async Task<string> GetHtmlAsync(int docId)
        {
            using var c = new HttpClient();
            c.DefaultRequestHeaders.Add("authority", "www.znakoved.ru");
            c.DefaultRequestHeaders.Add("method", "GET");
            c.DefaultRequestHeaders.Add("path", string.Format("/wtms.php?action=details&uid={0}", docId));
            c.DefaultRequestHeaders.Add("scheme", "https");
            c.DefaultRequestHeaders.Add("accept", "*/*");
            c.DefaultRequestHeaders.Add("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            c.DefaultRequestHeaders.Add("cookie", "VIEW-POPUP-MARK-2020-v2=1");
            c.DefaultRequestHeaders.Add("referer", string.Format(URL_TEMPLATE_REFER, docId));
            c.DefaultRequestHeaders.Add("sec-ch-ua", "Not A; Brand';v='99', 'Chromium';v='96', 'Google Chrome';v='96'");
            c.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            c.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
            c.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
            c.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
            c.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
            c.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36");
            c.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
            c.DefaultRequestHeaders.Add(nameof(HttpRequestHeader.Cookie), "VIEW-POPUP-MARK-2020-v2=1");
            var response = await c.GetAsync(string.Format(URL_TEMPLATE_DOWNLOAD, docId));
            string html = await response.Content.ReadAsStringAsync();
            return html.Replace("\"", "'");
        }

        /* parse tools */
        #region 
        static string GetString(Match m)
        {
            if (m.Success)
                return Regex.Match(m.Value, @">[\s\S]*?<").Value.Replace(">", "").Replace("<", "");
            return string.Empty;
        }

        static string GetPhrase(string html)
        {
            const string TEMPLATE_TM = @"<td class='TradeMark' colspan='2'>[\s\S]*?<";
            return GetString(new Regex(TEMPLATE_TM, RegexOptions.IgnoreCase).Match(html));
        }

        static int GetNumber(string html, int docId)
        {
            const string NUMBER_TM = @"Номер Регистрации[\s\S]*?url btn btn-danger btn-mini";
            const string NUMBER_TM_VALUE = @"e;'>[\s\S]*?<a";

            int res = -1;
            var m = new Regex(NUMBER_TM, RegexOptions.IgnoreCase).Match(html);
            if (!m.Success)
                throw new ParseException(docId.ToString(), "NUMBER");

            var mv = new Regex(NUMBER_TM_VALUE, RegexOptions.IgnoreCase).Match(m.Value);
            var sv = GetString(mv);

            if (int.TryParse(sv, out res))
                return res;

            throw new ParseException(docId.ToString(), "NUMBER");
        }

        static string GetRequestNumber(string html, int docId)
        {
            const string REQUEST_TM = @"Номер Заявления</span></td><td[\s\S]*?</td>";
            const string REQUEST_TM_VALUE = @"e;'>[\s\S]*?<";

            var m = new Regex(REQUEST_TM, RegexOptions.IgnoreCase).Match(html);
            if (!m.Success)
                return "";

            var mv = new Regex(REQUEST_TM_VALUE, RegexOptions.IgnoreCase).Match(m.Value);
            if (!mv.Success)
                throw new ParseException(docId.ToString(), "REQUESTNUMBER");

            return GetString(mv);
        }

        static string GetOwner(string html, int docId)
        {
            const string OWNER_TM = @"Заявитель</span></td><td[\s\S]*?</td>";
            const string OWNER_TM_VALUE = @"e;'>[\s\S]*?<";

            var m = new Regex(OWNER_TM, RegexOptions.IgnoreCase).Match(html);
            if (!m.Success)
                throw new ParseException(docId.ToString(), "OWNER");

            var mv = new Regex(OWNER_TM_VALUE, RegexOptions.IgnoreCase).Match(m.Value);
            if (!mv.Success)
                throw new ParseException(docId.ToString(), "OWNER");

            return GetString(mv);
        }

        static List<int> GetMktuList(string html, int docId)
        {
            const string MKTU_TM = @"МКТУ</span></td><td[\s\S]*?</a></td></tr>";
            const string MKTU_TM_A = @"<a[\s\S]*?</a>";

            var m = new Regex(MKTU_TM, RegexOptions.IgnoreCase).Match(html);
            if (!m.Success)
                return new List<int>();
            // throw new ParseException(docId, "MKTU");

            var mtch = new Regex(MKTU_TM_A, RegexOptions.IgnoreCase).Matches(m.Value);
            if (mtch.Count == 0)
                throw new ParseException(docId.ToString(), "MKTU");
            List<int> rslt = new List<int>();
            foreach (Match mt in mtch.Cast<Match>())
            {
                rslt.Add(int.Parse(GetString(mt)));
            }

            return rslt;
        }

        static string GetAdress(string html, int docId)
        {
            const string OWNER_TM = @"Заявителя</span></td><td[\s\S]*?</td>";
            const string OWNER_TM_VALUE = @"e;'>[\s\S]*?<";

            var m = new Regex(OWNER_TM, RegexOptions.IgnoreCase).Match(html);
            if (!m.Success)
                return "";

            var mv = new Regex(OWNER_TM_VALUE, RegexOptions.IgnoreCase).Match(m.Value);
            if (!mv.Success)
                throw new ParseException(docId.ToString(), "ADDRESS");

            return GetString(mv);
        }
        static DateTime GetRequestDate(string html, int docId)
        {
            const string REQUEST_DATE_TM = @"Дата Заявления</span></td><td[\s\S]*?</td>";
            const string REQUEST_DATE_VALUE_TM = @"middle;'[\s\S]*?</td>";
            var m = new Regex(REQUEST_DATE_TM, RegexOptions.IgnoreCase).Match(html);
            if (!m.Success)
                throw new ParseException(docId.ToString(), "REQUESTDATE");

            var mv = new Regex(REQUEST_DATE_VALUE_TM, RegexOptions.IgnoreCase).Match(m.Value);
            string sDate = GetString(mv);
            return DateTime.ParseExact(sDate, DATE_FORMAT, new CultureInfo(CULT_PROVIDER_NAME));
        }

        static DateTime GetTmDate(string html)
        {
            const string DATE_TM = @"Дата Регистрации</span></td><td[\s\S]*?</td>";
            const string DATE_VALUE_TM = @"middle;'[\s\S]*?</td>";
            var m = new Regex(DATE_TM, RegexOptions.IgnoreCase).Match(html);
            if (!m.Success)
                return DateTime.MinValue;

            var mv = new Regex(DATE_VALUE_TM, RegexOptions.IgnoreCase).Match(m.Value);
            string sDate = GetString(mv);

            return DateTime.ParseExact(sDate, DATE_FORMAT, new CultureInfo(CULT_PROVIDER_NAME));
        }
        static DateTime GetTmFinishDate(string html, int docId)
        {
            const string DATE_TM = @"Окончания Регистрации</span></td><td[\s\S]*?</td>";
            const string DATE_VALUE_TM = @"middle;'[\s\S]*?</td>";
            var m = new Regex(DATE_TM, RegexOptions.IgnoreCase).Match(html);
            if (!m.Success)
                throw new ParseException(docId.ToString(), "REQUESTDATE");

            var mv = new Regex(DATE_VALUE_TM, RegexOptions.IgnoreCase).Match(m.Value);
            string sDate = GetString(mv);
            return DateTime.ParseExact(sDate, DATE_FORMAT, new CultureInfo(CULT_PROVIDER_NAME));
        }
        static DateTime GetTmPriorityDate(string html)
        {
            const string DATE_TM = @"Приоритета</span></td><td[\s\S]*?</td>";
            const string DATE_VALUE_TM = @"middle;'[\s\S]*?</td>";
            var m = new Regex(DATE_TM, RegexOptions.IgnoreCase).Match(html);
            if (!m.Success)
                return DateTime.MinValue;

            var mv = new Regex(DATE_VALUE_TM, RegexOptions.IgnoreCase).Match(m.Value);
            string sDate = GetString(mv);
            return DateTime.ParseExact(sDate, DATE_FORMAT, new CultureInfo(CULT_PROVIDER_NAME));
        }
        #endregion



    }
}