using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.InsertDTOs
{
    public class MerchantAddDto
    {
        public string? StoreName { get; set; }
        public string userId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } =string.Empty;

        public string Email { get; set; } =string.Empty ;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public double? CostPerRefusedOrder { get; set; }

        public decimal MerchantPayingPercentageForRejectedOrders { get; set; }
        [Column(TypeName = "money")]
        public decimal? SpecialPickupShippingCost { get; set; }
        public List<SpecialPackages> SpecialPackages { get; set; }

        //  public decimal? RefusedOrderPercentage { get; set; }
        public City city { get; set; } 
        public Governorate governorate { get; set; }
        public List<Order> orders { get; set; }
        public ApplicationUser? User { get; set; }

    }
}
