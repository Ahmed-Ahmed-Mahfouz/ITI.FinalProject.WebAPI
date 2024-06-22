using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Shipping
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
    }
}
