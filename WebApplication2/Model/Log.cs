using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Model
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DoneToday { get; set; }
        public string DoingTomorrow { get; set; }
    }
}
