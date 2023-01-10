using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItZnak.PatentsDtoLibrary.Types.Members
{
    public class RequestState{
        //public PatentService.Types.RequestState StateId {get;set;}
        public int StateId {get;set;}
        public string Name {get;set;}="";
        public string ClientFormTips {get;set;}=""; // подсказка на форме заявки 
        public string MailTemplate {get;set;}=""; // шаблон письма отправляемого клиенту   
         public string MailSubject {get;set;}=""; // шаблон темы   
        public bool ShowStageInClientForm {get;set;}=  false;// показывать этап в форме заявки 
    }

}