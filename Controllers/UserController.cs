using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_mssql_dapper.DTOs;
using api_mssql_dapper.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace api_mssql_dapper.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IBaseRepository repository;
        private readonly IWebHostEnvironment _env;
        public UserController(IBaseRepository repository, IWebHostEnvironment env)
        {
            this.repository = repository;
            _env = env;
        }

        [HttpGet]
        // public IEnumerable<User> GetAllUser()
        // {
        //     return SQLHelper.ExecQueryData<User>("select * from tbl_user");
        // }
        public async Task<IEnumerable<UserDTO_get>> GetAllUserAsync()
        {
            var result = await repository.GetAllAsync<User>();
            var newResult = result.Select(ud => ud.ToDTOGet());
            return newResult;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetThisOneAsync(int id)
        {
            var item = await repository.GetAsync<User>(id);

            if (item is null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> DeleteAsync(int id)
        {
            var rep = await repository.DeleteAsync(id);
            return rep;
        }

        [HttpPost]
        public async Task<Response> AddAsync(User item)
        {//public async Task<ActionResult> AddAsync(User item)
            // await repository.SaveAsync(item);
            // return CreatedAtAction(nameof(GetThisOneAsync), new { id = item.id }, item);
            return await repository.SaveAsync(item);
        }

        [HttpPut]
        public async Task<Response> UpdateAsync(UserDTO_update item)
        {
            var existsItem = await repository.GetAsync<User>(item.id_user);
            if (existsItem is null)
            {
                return new Response() { Status = "ERROR", Description = "User not found !" };
            }

            return await repository.SaveAsync<UserDTO_update>(item);
        }
    }
}