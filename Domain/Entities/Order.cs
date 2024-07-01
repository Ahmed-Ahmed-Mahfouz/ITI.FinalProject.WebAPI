using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        // Properties
        public int Id { get; set; }
        public string Type { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public string Phone { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public string VillageAndStreet { get; set; }
        public bool? ShippingToVillage { get; set; }
        public decimal? OrderMoneyReceived { get; set; }
        public decimal? ShippingMoneyReceived { get; set; }

        // Calculated properties
        public decimal TotalPrice => Products.Sum(p => p.Price);
        public decimal TotalWeight => Products.Sum(p => p.Weight);

        // Foreign keys
        [ForeignKey("merchant")]
        public string MerchantId { get; set; }
        [ForeignKey("governorate")]
        public int GovernorateId { get; set; }
        [ForeignKey("city")]
        public int CityId { get; set; }
        [ForeignKey("payment")]
        public int PaymentId { get; set; }
        [ForeignKey("shipping")]
        public int ShippingId { get; set; }

        // Navigation properties
        public OrderStatus Status { get; set; }
        public Merchant Merchant { get; set; }
        public Governorate Governorate { get; set; }
        public City City { get; set; }
        public Payment Payment { get; set; }
        public Shipping Shipping { get; set; }
        public List<Product> Products { get; set; }
    }
}
