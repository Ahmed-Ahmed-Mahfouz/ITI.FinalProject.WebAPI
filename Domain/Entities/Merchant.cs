using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Merchant
    {
        public string? StoreName { get; set; }
        public int? GovernorateId { get; set; }
       // public virtual Governorate? Governorate { get; set; }

        public int? CityId { get; set; }
        //public virtual City? City { get; set; }

        public decimal CostperRefusedOrder { get; set; }

        public int? RefusedOrderPercentage { get; set; }

    }
}
