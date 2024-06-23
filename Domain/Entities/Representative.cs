﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Representative
    {
        public string DiscountType { get; set; }
        public int CompanyPercetage { get; set; }
        [ForeignKey("user")]
        [Key]
        public string userId { get; set; }

        public List<GovernorateRepresentatives> governorates { get; set; }
        public ApplicationUser user { get; set; }
    }
}