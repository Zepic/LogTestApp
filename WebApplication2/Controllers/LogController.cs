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
        //sql connection
        string _connStr =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestLogDatabase;Integrated Security=True;"; // 
        string create = @"INSERT INTO LogDatabase(Date, DoneToday, DoingTomorrow)
                           VALUES (@Date, @DoneToday, @DoingTomorrow)";

        string readAll = @"SELECT Id, Date, DoneToday, DoingTomorrow
                            FROM LogDatabase";

        string readOne = @"SELECT Id, Date, DoneToday, DoingTomorrow
                            FROM LogDatabase
                            WHERE Id = @Id";

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
        public async Task<Log> ReadLast()
        {
            var conn = new SqlConnection(_connStr);
            IEnumerable<Log> Logs = await conn.QueryAsync<Log>(readAll);
            foreach (var log in Logs)
            {
                if (log != null)
                {
                    _logList.Add(new Log()
                    {
                        Id = log.Id,
                        Date = log.Date,
                        DoneToday = log.DoneToday,
                        DoingTomorrow = log.DoingTomorrow
                    });
                }
            }
            return await Task.Run(() => 
                Logs.FirstOrDefault(p => p.Id == _logList.Count)
            );
        }

        /*
            foreach (var log in Logs)
            {
                if (log != null)
                {
                    _logs.Add(new Log()
                    {
                        Id = log.Id, Date = log.Date, DoneToday = log.DoneToday, DoingTomorrow = log.DoingTomorrow
                    });
                }
            }
         */

        // int rowsInserted1 = await conn.ExecuteAsync(create, new { Date = new DateTime(), DoneToday = "", DoingTomorrow = "" });
        // IEnumerable<Log> Logs = await conn.QueryAsync<Log>(readAll);
        // Log terje = await conn.QueryFirstOrDefaultAsync<Log>(readOne, new { Id = 0 });


        /*
         *int rowsInserted1 = await conn.ExecuteAsync(create, new { FirstName = "Terje", LastName = "Kolderup", BirthYear = 1975 });
         * IEnumerable<Person> persons = await conn.QueryAsync<Person>(readMany);
         *  Person terje = await conn.QueryFirstOrDefaultAsync<Person>(readOneByName, new { FirstName = "Terje" });
         *
         */
    }

}
