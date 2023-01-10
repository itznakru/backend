using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PatentService.Types
{
    public enum ModifyErrorCode { OK = 0, VectorError = 1, EmptyTmPhrase = 2 }
    public enum TypeImageFieldInfo { Undefined = 0, IsText = 1, IsImage = 2 }
    public class TradeMark
    {
        public ObjectId _id { get; set; }
        public string RequestNumber { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RequestDate { get; set; } = DateTime.MinValue; //Дата подачи заявки: (220)
        public int TMNumber { get; set; }//слоган фраза и т.п.
        public string TMPhrase { get; set; }// 
        public int DocId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TMDate { get; set; } = DateTime.MinValue; //Дата гос. регистрации:(151)

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TMPublicDate { get; set; } = DateTime.MinValue; //Дата гос. регистрации:(450)

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TMFinishDate { get; set; } = DateTime.MinValue; //Дата истечения:(181)

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TMPriorityDate { get; set; } = DateTime.MinValue; //Приоритет товарного знака:
        public List<int> MktuList { get; set; } = new List<int>();
        public string NotGuarded { get; set; }
        public string TMOwner { get; set; } //Правообладатель (732)
        public string Address { get; set; } //Правообладатель (750)
        public string Url { get; set; } //картинка URL
        public string Image { get; set; }
        public DateTime MiniigDate { get; set; }
        public byte[] Vector { get; set; } = null;
        public TypeImageFieldInfo TextImageInfo { get; set; } = TypeImageFieldInfo.Undefined; // поле информации типе картинки
        public int ModifyErrorCode { get; set; } // 1- ошибка при формировании вектора 2-TmPhrase - отсутсвует
        public bool IsVectorFilled { get; set; } = false; // вектор построен  
        public bool IsPhraseFilled { get; set; } = false; // фраза заполнена
    }
}