using System.Collections.Generic;
using System.Threading.Tasks;

namespace api_mssql_dapper
{
    public interface IBaseRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<Response> DeleteAsync(int id);
        Task<Response> SaveAsync<T>(T item);
    }
}