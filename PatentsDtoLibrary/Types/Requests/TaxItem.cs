using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using ItZnak.PatentsDtoLibrary.TypeEnums;

namespace ItZnak.PatentsDtoLibrary.Types
{
    public class TaxItem
    {
      public int _id{get;set;}

      public TaxType ComissionType{get;set;}

      public string Name {get;set;}="";

      public string Describe {get;set;}="";
      public int Value {get;set;}
    }
}