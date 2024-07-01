using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DisplayDTOs
{
    public class DisplayOrderDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Client_Name { get; set; }
        public DateTime Date { get; set; }
        public string Phone { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public decimal Total_Price { get; set; }
        public decimal Total_Weight { get; set; }
        public string VillageAndStreet { get; set; }
        public bool? ShippingToVillage { get; set; }
        public string StoreName { get; set; }
        public string GovernorateName { get; set; }
        public string CityName { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingMethod { get; set; } 
        public OrderStatus Status { get; set; }
        public List<DisplayProductDTO> Products { get; set; }
    }
}
