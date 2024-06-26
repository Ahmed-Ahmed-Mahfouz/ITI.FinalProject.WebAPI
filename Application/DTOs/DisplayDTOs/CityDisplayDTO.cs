using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DisplayDTOs
{
    public class CityDisplayDTO
    {
        public string name { get; set; }
        public Status status { get; set; }
        public decimal normalShippingCost { get; set; }
        public decimal pickupShippingCost { get; set; }
        public int stateId { get; set; }
        public string branchName { get; set; }
        public List<Merchant> cityMerchants { get; set; }
        public List<Order> cityOrders { get; set; }
    }
}
