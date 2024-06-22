using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Client_Name { get; set; }
        public DateTime Date { get; set; }
        public int Phone { get; set; }
        public int? Phone2 { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public decimal Total_Price { get; set; }
        public decimal Total_Weight { get; set;}
        public string VillageAndStreet { get; set; }
        public char? ShippingToVillage { get; set; }
        public int Merchant_Id { get; set; }
        public int Governorate_Id { get; set; }
        public int City_Id { get; set; }
    }
}
