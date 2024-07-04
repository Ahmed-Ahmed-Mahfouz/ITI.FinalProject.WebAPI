using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UpdateDTOs
{
    public class UpdateShippingDTO
    {
        public int Id { get; set; }
        public ShippingTypes? ShippingType { get; set; }
        public decimal? Price { get; set; }
    }
}
