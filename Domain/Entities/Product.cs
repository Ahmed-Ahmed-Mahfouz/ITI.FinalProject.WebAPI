using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        // Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? StatusNote { get; set; }

        // Foreign keys
        [ForeignKey("order")]
        public int OrderId { get; set; }

        // Navigation properties
        public OrderStatus ProductStatus { get; set; }
        public Order Order { get; set; }    
    }
}
