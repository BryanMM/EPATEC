using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Order
    {
        public string Products { get; set; }
        public string Amount { get; set; }
        public int O_ID { get; set; }
        public int OPriority { get; set; }
        public string OStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string OPlatform { get; set; }
        public int S_ID = 100;
        public int C_ID = 1;

    }
}
