using Domain.Enums;
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
        public decimal Price { get; set; }
        public ShippingTypes ShippingType { get; set; }
        public List<Order> Orders { get; set; }
    }
}
