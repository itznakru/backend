using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItZnak.PatentsDtoLibrary.Types.Members
{
    public class WizardStageSetting{
        public int _id {get;set;}
        public string Name {get;set;}="";
        public string HelpText {get;set;}=""; // подсказка на форме заявки 
        public bool UseStage {get;set;}=true;
    }

}