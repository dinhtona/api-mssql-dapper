using System.Collections.Generic;
using System.Threading.Tasks;
using api_mssql_dapper.DTOs;
using api_mssql_dapper.Models;

namespace api_mssql_dapper
{
    public class UserRepository : IBaseRepository
    {
        public async Task<Response> DeleteAsync(int id)
        {
            //throw new NotImplementedException();
            return await SQLHelper.ExecProcedureDataFirstOrDefaultAsync<Response>("USP_User_Delete", new { id });
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var result = await SQLHelper.ExecProcedureDataAsync<T>("USP_User_Get");
            return result;
        }

        public async Task<T> GetAsync<T>(int id)
        {
            //throw new NotImplementedException();
            return await SQLHelper.ExecProcedureDataFirstOrDefaultAsync<T>("USP_User_Get", new { id });
        }

        public async Task<Response> SaveAsync<T>(T item)
        {
            UserDTO_update obj = null;
            if (item is UserDTO_update)
            {
                obj = item as UserDTO_update;
                return await SQLHelper.ExecProcedureDataFirstOrDefaultAsync<Response>("USP_User_Save",
                new
                {
                    id = obj.id_user,
                    f_name = obj.last_name,
                    l_name = obj.fisrt_name,
                    dob = obj.date_of_birth,
                    email = obj.email_address,
                    phone = obj.phone_number,
                    username = "ROOT"
                }
                );
            }
            else
            {
                return new Response() { Status = "ERROR", Description = "Expect User but an item is not User !" };
            }
        }
    }
}