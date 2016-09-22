using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace Core.Models
{
    public class Ordering
    {
        public string attr { get; set; }
        public string avalues { get; set; }
        public Order order { get; set; }
    }
}
