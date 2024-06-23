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
        public int id { get; set; }
        public PaymentTypes paymentType { get; set; }
        public List<Order> paymentorders { get; set;}

    }
}
