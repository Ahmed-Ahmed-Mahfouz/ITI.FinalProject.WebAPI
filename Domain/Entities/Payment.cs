using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Payment
    {
        public int id { get; set; }
        public PaymentTypes paymentType { get; set; }
    }
}
