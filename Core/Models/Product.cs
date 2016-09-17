using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Product
    {
        public int PR_ID { get; set; }
        public int Price { get; set; }
        public bool Extent { get; set; }
        public string PDescription { get; set; }
        public int Quantity { get; set; }
        public string PName { get; set; }

    }
}
