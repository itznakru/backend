using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItZnak.PatentsDtoLibrary.Types.Members
{
    // Список услуг
    public class MemberClientServiceParam{
        public int _id {get;set;}
        public string Name{get;set;}="";
        public int MktuCount{get;set;} =0;//триггерное количество МКТУ 
        public int Price {get;set;} =0; // стоимость за единицу
        public bool IsObligatory {get;set;} =false; // обязательный параметр 
        public bool Enabled {get;set;}=true; // предоставлять или не предоставлять услугу 
        public string Describe {get;set;}=""; // описание услуги 
        public bool IsForImage {get;set;}=true; //сервис применим для ТЗ картинки
        public bool IsForWord {get;set;}=true; // сервис доступен для слов 
        public bool IsForCombineImageWord {get;set;}=false;// для комбинированого знака слово+картинка
        public bool IsForQuick {get;set;}=false; //сервис применим при ускоренном рассмотрениее

        public bool IsCalculate {get{return MktuCount>0;} set{;}} // обязательный параметр 
    }

    // глобальный справочник доступных услуг
    public class MemberClientServiceParamDictionary{
        static MemberClientServiceParamDictionary _inst;
        private readonly List<MemberClientServiceParam> _list = new();
        public MemberClientServiceParamDictionary(){
            _list.Add(new MemberClientServiceParam(){_id=0,Name="Подготовка и подача заявки",MktuCount=2,Price=1000,IsObligatory=true});
            _list.Add(new MemberClientServiceParam(){_id=1,Name="Ведение дополнительного МКТУ",MktuCount=1,Price=1000,IsObligatory=false});
            _list.Add(new MemberClientServiceParam(){_id=2,Name="Электронное делопроизводство",MktuCount=0,Price=10000,IsObligatory=false});
        }
        public MemberClientServiceParam GetParam(int id){
            return _list.First(_=>_._id==id);
        }

        public static  MemberClientServiceParamDictionary Instance {
            get{
                return _inst ??= new MemberClientServiceParamDictionary();
            }
            set{}
        }


    }
}