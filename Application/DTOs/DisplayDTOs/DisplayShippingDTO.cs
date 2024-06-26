using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DisplayDTOs
{
    public class DisplayShippingDTO
    {
        public int Id { get; set; }
        public ShippingTypes ShippingMethod { get; set; }
        public decimal Price { get; set; }
    }
}
