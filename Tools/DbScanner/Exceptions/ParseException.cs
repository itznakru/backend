namespace DbScanner.Exceptions{
    public class ParseException : Exception
    {
        private readonly string _docId;
        private readonly string _field;
        public ParseException(string docId, string field) : base() {
            _docId = docId; _field = field;
        }
        public string DocId {get{ return _docId; } }
        public string Field { get { return _field; } }
    }

    public class ParseTimeOutException : Exception
    {
        public ParseTimeOutException() : base()
        {
        }

        public ParseTimeOutException(string message) : base(message)
        {
        }

        public ParseTimeOutException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}