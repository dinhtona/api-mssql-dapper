using api_mssql_dapper.DTOs;
using api_mssql_dapper.Models;

namespace api_mssql_dapper
{
    public static class Extensions
    {
        public static UserDTO_get ToDTOGet(this User user)
        {
            return new UserDTO_get()
            {
                id_user = user.id,
                fisrt_name = user.f_name,
                last_name = user.l_name,
                date_of_birth = user.dob,
                phone_number = user.phone,
                email_address = user.email,
                username = user.created_user,
                created = user.created_date
            };

        }

        public static UserDTO_update ToDTOUpdate(this User user)
        {
            return new UserDTO_update()
            {
                id_user = user.id,
                fisrt_name = user.f_name,
                last_name = user.l_name,
                date_of_birth = user.dob,
                phone_number = user.phone,
                email_address = user.email,
                username = user.created_user
            };

        }
    }
}