using PatentService.Types;

namespace DbScanner.Actions{
    interface ISiteParser{
        Task<TradeMark> ProcessAsync (int DocId, List<Tuple<string,string>> proxyList);
    }
}