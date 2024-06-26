using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.InsertDTOs
{
    public class InsertOrderDTO
    {
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
        public string MerchantId { get; set; }
        public int GovernorateId { get; set; }
        public int CityId { get; set; }
        public int PaymentId { get; set; }
        public int ShippingId { get; set; }
        public OrderStatus Status { get; set; }
        public List<InsertProductDTO> Products { get; set; }
    }
}
