namespace  ItZnak.PatentsDtoLibrary.TypeEnums
{
    public enum FaceType {IP=0,PUBLIC_ORGANIZATION=1,INDIVIDUAL=3, NONE=100}

    public enum TmType { WORD = 0, IMAGE = 1, MIX = 2 }

    public enum DocType {MAIL=0,BILL=1,CONTRACT=2,HTML_MAIL_BILL=3,HTML_MAIL_REQUEST=4,
                        HTML_TERMS_OF_USE=5, HTML_INFORMATION_AGREEMENT=6,HTML_NEW_PARTNER=7}

     // подача завяки, получение сертичфиката, надбавка за каждый дополнительный МКТУ
    public enum TaxType{ApplicationTax, ExpertTax, CertificateTax, IssueTax, FastTax}
}
