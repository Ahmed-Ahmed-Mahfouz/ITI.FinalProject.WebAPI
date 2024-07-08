using Application.DTOs.InsertDTOs;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UpdateDTOs
{
    public class MerchantUpdateDto
    {
        public string Id { get; set; }

        public string? StoreName { get; set; }
        //public string userId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        //public double? CostPerRefusedOrder { get; set; }
        public decimal MerchantPayingPercentageForRejectedOrders { get; set; }
        //[Column(TypeName = "money")]
        public decimal? SpecialPickupShippingCost { get; set; }
        public Status Status { get; set; }

        public int cityID { get; set; }
        public int branchId { get; set; }
        public string cityName { get; set; }
        public int governorateID { get; set; }
        //public string governerateName { get; set; }
        public List<SpecialPackageUpdateDTO> SpecialPackages { get; set; }


        ////  public decimal? RefusedOrderPercentage { get; set; }
        //public List<Order> orders { get; set; }
        //public ApplicationUser? User { get; set; }

    }
}
