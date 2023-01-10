using MongoDB.Bson;
using System;
using System.Collections.Generic;
using ItZnak.PatentsDtoLibrary.TypeEnums;

namespace ItZnak.PatentsDtoLibrary.Types.Members
{
    // Корневой класс - описывает участника системы
    public class MemberDocumentTemplate{
        public int _id {get;set;}
        public string  Name{get;set;}="";
        public string  File{get;set;}="";
        public string FileName{get;set;}="";
        public DateTime Date {get;set;}
        public int Size {get;set;}
        public FaceType FaceType {get;set;} =FaceType.PUBLIC_ORGANIZATION;
        public DocType DocType {get;set;} =DocType.MAIL;
    }
}