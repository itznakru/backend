using MongoDB.Bson;

using System;
using System.Collections.Generic;

namespace ItZnak.PatentsDtoLibrary.Types.Members
{
    public class MemberClientServiceItem
    {
        public MemberClientServiceParam Service {get;set;}
        // public int ServiceId {get;set;}
        // public int MktuCount {get;set;}
        // public bool On{get;set;}
        public bool IsPaid {get;set;}
        public string PaidDocument {get;set;}="";
        public int Summ {get;set;}
        public bool IsSelected {get;set;}= false; // используется как признак того, что пользователь выбрал значение
    }
}