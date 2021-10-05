namespace api_mssql_dapper{
    public class Response
    {
        /// <summary>Value is: OK or ERROR are common</summary>
        public string Status { get; set; }
        public string Description { get; set; }

        /// <summary>Data Json Type</summary>
        public object Data { get; set; }
    }

    public enum StatusValue
    {
        OK,
        ERROR
    }
}