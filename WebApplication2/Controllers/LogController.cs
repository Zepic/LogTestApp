using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Model;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        string _connStr =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestLogDatabase;Integrated Security=True;";
        string create = @"INSERT INTO LogDatabase(Date, DoneToday, DoingTomorrow)
                           VALUES (@Date, @DoneToday, @DoingTomorrow)";

        string readAll = @"SELECT Id, Date, DoneToday, DoingTomorrow
                            FROM LogDatabase";

        string readLast = @"Select * from LogDatabase where Id = (select max(Id) from LogDatabase)";

        List<Log> _logList = new List<Log>();
        [HttpPost]
        public async Task<int> CreateLog(Log log)
        {
            var conn = new SqlConnection(_connStr);
            int rowsInserted1 = await conn.ExecuteAsync(create, log);
            return await Task.Run( () => 1);
        }

        public async Task<IEnumerable<Log>> ReadAll()
        {
            var conn = new SqlConnection(_connStr);
            await conn.QueryAsync<Log>(readAll);

            return await Task.Run(() => _logList);
        }

        [HttpGet]
        public async Task<IEnumerable<Log>> ReadLast()
        {
            var conn = new SqlConnection(_connStr);
            var log = await conn.QueryAsync<Log>(readLast);
            
            return await Task.Run(() => log);
        }

    }

}
