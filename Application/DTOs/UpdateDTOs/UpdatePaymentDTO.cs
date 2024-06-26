using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UpdateDTOs
{
    public class UpdatePaymentDTO
    {
        public int Id { get; set; }
        public PaymentTypes? PaymentType { get; set; }
    }
}
