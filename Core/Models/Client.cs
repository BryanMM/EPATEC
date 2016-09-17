using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Client
    {
        public int C_ID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string CAddress { get; set; }
        public int Phone { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Penalization { get; set; }
        public string CPassword { get; set; }

    }
}
