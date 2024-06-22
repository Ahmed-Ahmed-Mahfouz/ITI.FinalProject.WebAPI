﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Merchant
    {
        public string? StoreName { get; set; }
        [ForeignKey("governorate")]
        public int? GovernorateId { get; set; }
        // public virtual Governorate? Governorate { get; set; }
        [ForeignKey("city")]
        public int? CityId { get; set; }
        //public virtual City? City { get; set; }

        public decimal CostperRefusedOrder { get; set; }

        public int? RefusedOrderPercentage { get; set; }

        [ForeignKey("user")]
        [Key]
        public string userId { get; set; }

        public City city { get; set; }
        public Governorate governorate { get; set; }
        public List<Order> orders { get; set; }
        public ApplicationUser user { get; set; }


    }
}
