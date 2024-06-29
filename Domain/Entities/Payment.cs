using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public PaymentTypes PaymentType { get; set; }
        public List<Order> PaymentOrders { get; set;}

    }
}
