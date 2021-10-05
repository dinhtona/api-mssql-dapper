using System;

namespace api_mssql_dapper.Models
{
    public class User
    {
        public int id { get; set; }

        public string f_name { get; set; }

        public string l_name { get; set; }

        public DateTime? dob { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public DateTime? created_date { get; set; }

        public string created_user { get; set; }

        public DateTime? updated_date { get; set; }

        public string updated_user { get; set; }

    }

}