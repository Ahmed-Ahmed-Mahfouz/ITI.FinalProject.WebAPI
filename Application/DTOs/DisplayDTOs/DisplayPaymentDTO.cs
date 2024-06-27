using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DisplayDTOs
{
    public class DisplayPaymentDTO
    {
        public int Id { get; set; }
        public PaymentTypes PaymentMethod { get; set; }
    }
}
