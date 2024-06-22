using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public Status status { get; set; }
        public decimal normalShippingCost { get; set; }
        public decimal pickupShippingCost { get; set;}
    }
}
