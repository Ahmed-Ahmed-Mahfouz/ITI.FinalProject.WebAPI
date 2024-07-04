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
        public int Id { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public string Phone { get; set; }
        public string? Phone2 { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public string VillageAndStreet { get; set; }
        public bool ShippingToVillage { get; set; }
        public PaymentTypes PaymentType { get; set; }
        public OrderStatus Status { get; set; }
        public OrderTypes Type { get; set; }
        [Column(TypeName = "money")]
        public decimal? OrderMoneyReceived { get; set; }
        [Column(TypeName = "money")]
        public decimal? ShippingMoneyReceived { get; set; }

        // Still to be added to database

        [Column(TypeName = "money")]
        public decimal ShippingCost { get; set; }

        // Calculated properties
        [Column(TypeName = "money")]
        public decimal TotalPrice => Products.Sum(p => p.Price * p.Quantity);
        public decimal TotalWeight => Products.Sum(p => p.Weight * p.Quantity);

        [ForeignKey("merchant")]
        public string? MerchantId { get; set; }
        [ForeignKey("governorate")]
        public int GovernorateId { get; set; }
        [ForeignKey("city")]
        public int CityId { get; set; }
        [ForeignKey("shipping")]
        public int ShippingId { get; set; }
        [ForeignKey("branch")]
        public int BranchId { get; set; }
        [ForeignKey("representative")]
        public string RepresentativeId { get; set; }

        public Merchant merchant { get; set; }
        public Governorate governorate { get; set; }
        public City city { get; set; }
        public Shipping shipping { get; set; }
        public Branch branch { get; set; }
        public Representative representative { get; set; }
        public List<Product> Products { get; set; }
    }
}
