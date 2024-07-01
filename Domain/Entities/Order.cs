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
        [ForeignKey("merchant")]
        public string MerchantId { get; set; }
        [ForeignKey("governorate")]
        public int GovernorateId { get; set; }
        [ForeignKey("city")]
        public int CityId { get; set; }
        [ForeignKey("payment")]
        public int paymentId { get; set; }
        [ForeignKey("shipping")]
        public int shippingId { get; set; }
        public OrderStatus Status { get; set; }
        public Merchant merchant { get; set; }
        public Governorate governorate { get; set; }
        public City city { get; set; }
        public Payment payment { get; set; }
        public Shipping shipping { get; set; }
        public List<Product> products { get; set; }
    }
}
