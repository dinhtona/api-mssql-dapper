using System;

namespace api_mssql_dapper.Models
{
    public class UserDTO_get
    {
        public int id_user { get; set; }

        public string fisrt_name { get; set; }

        public string last_name { get; set; }

        public DateTime? date_of_birth { get; set; }

        public string email_address { get; set; }

        public string phone_number { get; set; }

        public DateTime? created { get; set; }

        public string username { get; set; }
    }

}