using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.InsertDTOs
{
    public class InsertShippingDTO
    {
        public ShippingTypes ShippingType { get; set; }
        public decimal Price { get; set; }
    }
}
