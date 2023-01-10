using MongoDB.Bson;
using ItZnak.PatentsDtoLibrary.Types.Members;
using System;
using System.Collections.Generic;

namespace ItZnak.PatentsDtoLibrary.Types.Members
{
    // Корневой класс - описывает участника системы
    public class MemberClient
    {
        public ObjectId _id { get; set; }
        public Identity ContactFace {get;set;}
        public DateTime RegistredDate { get; set; } = DateTime.Now;
    }
}