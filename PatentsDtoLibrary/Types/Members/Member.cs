using MongoDB.Bson;
using ItZnak.PatentsDtoLibrary.Types.Members;
using ItZnak.PatentsDtoLibrary.TypeEnums;
using System;
using System.Collections.Generic;

namespace ItZnak.PatentsDtoLibrary.Types.Members
{
    // Корневой класс - описывает участника системы
    public class Member{
        
        public ObjectId _id { get; set; }
        public Identity ContactFace {get;set;} // контактное лицо
        public string Key {get;set;}=""; // ключ клиента
        public Dictionary<string,string> Settings {get;set;}= new System.Collections.Generic.Dictionary<string, string>(){
                    {"WizardPage","http://127.0.0.1:8080/app/wizard-plugin/test-site/"},
                    {"Box","a.grischko2016@yandex.ru"},
                    {"Host","smtp.yandex.com"},
                    {"Port","25"},
                    {"Password","Nidecker1"},
                    {"AfterCreateRequestMsg","subject:Новая заявка body:В системе появилась новая заявка @Number"}
                };
        public List<MemberDocumentTemplate> DocumentTemplates{get;set;} = new List<MemberDocumentTemplate>(){
                                                                                new MemberDocumentTemplate(){_id=0,
                                                                                    Name="Шаблон счета (юр. лицо)",
                                                                                    Date=DateTime.MinValue, 
                                                                                    DocType=DocType.BILL,
                                                                                    FaceType=FaceType.PUBLIC_ORGANIZATION},

                                                                                new MemberDocumentTemplate(){_id=1,Name="Шаблон счета (физ. лицо)",
                                                                                                            Date=DateTime.MinValue, 
                                                                                                            DocType=DocType.BILL, 
                                                                                                            FaceType=FaceType.INDIVIDUAL},

                                                                                new MemberDocumentTemplate(){_id=2,Name="Шаблон договора",
                                                                                                            Date=DateTime.MinValue, 
                                                                                                            DocType=DocType.CONTRACT, 
                                                                                                            FaceType=FaceType.PUBLIC_ORGANIZATION},

                                                                                new MemberDocumentTemplate(){_id=3,Name="HTML шаблон заявка",
                                                                                                            Date=DateTime.MinValue, 
                                                                                                            DocType=DocType.HTML_MAIL_REQUEST,
                                                                                                            FaceType=FaceType.NONE },
                                                                                                            
                                                                                new MemberDocumentTemplate(){_id=4,Name="HTML шаблон счет",
                                                                                                            Date=DateTime.MinValue, 
                                                                                                            DocType=DocType.HTML_MAIL_BILL, 
                                                                                                            FaceType=FaceType.NONE},
                                                                                };  // шаблоны документов (bill|contract)
        public string  Name {get;set;}=""; 
        public List<MemberClient> Clients {get;set;} = new List<MemberClient>();// конечные клиенты
        // предоставляемые услуги
        public List<MemberClientServiceParam> ServicesSettings {get;set;}=new List<MemberClientServiceParam>(){
            new MemberClientServiceParam(){_id=0,Name="Подготовка и подача заявки"},
            new MemberClientServiceParam(){_id=1,Name="Сопровождение регистрации товарного знака"},
            new MemberClientServiceParam(){_id=2,Name="Анализ словесного товарного знака на охраноспособность до 5 классов МКТУ"},
            new MemberClientServiceParam(){_id=3,Name="Анализ комбинированного товарного знака на охраноспособность до 5 классов МКТУ"},
            new MemberClientServiceParam(){_id=4,Name="Анализ товарного знака за каждый класс МКТУ свыше 5"},
            new MemberClientServiceParam(){_id=5,Name="Сопровождение ускоренного оформления"}
        };

        // страница на которой садатся скрипты wizard
        public string LandingWizardPage {get;set;}="";
        /// суммы поналогам и сборам    
        public List<TaxItem> TaxSettings {get;set;} =new List<TaxItem>(){
                                                             new TaxItem(){
                                                                 _id=0,
                                                                 Name="Подача заявки",
                                                                 Describe="Описание подачи заявки",
                                                                 ComissionType=TaxType.ApplicationTax,
                                                                 Value=3500},
                                                            new TaxItem(){
                                                                 _id=1,
                                                                 Name="Экспертиза обозначения",
                                                                 Describe="Описание Экспертиза обозначения",
                                                                 ComissionType=TaxType.ExpertTax,
                                                                 Value=11500},
                                                            new TaxItem(){
                                                                 _id=2,
                                                                 Name="Регистрация товарного знака",
                                                                 Describe="Описание Регистрация товарного знака",
                                                                 ComissionType=TaxType.CertificateTax,
                                                                 Value=16000},
                                                            new TaxItem(){
                                                                 _id=3,
                                                                 Name="Выдача свидетельства",
                                                                 Describe="Описание Выдача свидетельства",
                                                                 ComissionType=TaxType.IssueTax,
                                                                 Value=2000},
                                                            new TaxItem(){
                                                                 _id=4,
                                                                 Name="Ускоренное оформление",
                                                                 Describe="Описание Ускоренное оформление",
                                                                 ComissionType=TaxType.FastTax,
                                                                 Value=94000},     

                                                            // new ComissionItem(){ComissionType=ComissionType.CertificateTax,Value=12600},
                                                            // new ComissionItem(){ComissionType=ComissionType.AddForNextMktu,Value=1250}
                                                        };// налоги и сборы 

        // настройки этапов
        public List<RequestState> StateSettings {get;set;}
        // настройки этапов WIZARD
        public List<WizardStageSetting> WizardSettings {get;set;}= new List<WizardStageSetting>(){
            new WizardStageSetting(){_id=0,Name="Создание профиля", HelpText="Комментарий по соданию профиля"},
            new WizardStageSetting(){_id=1,Name="Заявитель", HelpText="Комментарий по выбору клиента"},
            new WizardStageSetting(){_id=2,Name="Загрузка знака", HelpText="Комментарий по загрузке знака"},
            new WizardStageSetting(){_id=3,Name="Выбор МКТУ", HelpText="Комментарий по выбору МКТУ"},
            new WizardStageSetting(){_id=4,Name="Выбор услуг", HelpText="Комментарий по выбору услуг "},
            new WizardStageSetting(){_id=5,Name="Итоговая страница", HelpText="Комментарий по итоговой странице "},
            new WizardStageSetting(){_id=6,Name="Закрывашка", HelpText="Комментарий по странице закрывашке"},
        };
        // ассистент указываемый в wizard
        public WizardAssistantInfo Assistant {get;set;}= new WizardAssistantInfo();

        // значение организации родителя
        public string ParentMemberKey {get;set;}="";
    }

}