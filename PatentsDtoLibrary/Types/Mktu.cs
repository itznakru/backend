
using MongoDB.Bson;
namespace ItZnak.PatentsDtoLibrary.Types{
     public class MktuClass{
        const string SEPARATOR = "\t";
        const char TAB_SEPARATOR = '\u0009';
        public ObjectId  _id {get;set;}
        public List<MktuItem> Items {get;set;}= new List<MktuItem>();
        public int Code {get;set;}
        public string ShortDescribe {get;set;}="";
         public string Describe {get;set;}="";

        public void  Update(string v){
            if(v?.Length == 0)
                return;

            _id= ObjectId.GenerateNewId();
            var d= v.Split(SEPARATOR.ToCharArray());
            Code=int.Parse(d[0]);
            string s=d[2].Replace("Класс","")
                         .Replace(d[0],"")
                         .Replace(TAB_SEPARATOR.ToString(),"")
                         .Substring(2);

            Describe=s.Substring(0,s.Length-1);
        }

    }
    public class MktuItem{
        const string SEPARATOR = "\t";
        public  MktuItem(string v){
            var d= v.Split(SEPARATOR.ToCharArray());
                    ClassCode=d[0];
                    Code=int.Parse(d[1]);
                    Text=d[2];
        }

        public string ClassCode{get;set;}
        public int Code {get;set;}
        public string Text {get;set;}
    }
}