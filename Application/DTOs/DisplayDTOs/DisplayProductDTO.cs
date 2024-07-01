using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DisplayDTOs
{
    public class DisplayProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public OrderStatus ProductStatus { get; set; }
        public string? StatusNote { get; set; }
        public string ClientName { get; set; }
    }
}
