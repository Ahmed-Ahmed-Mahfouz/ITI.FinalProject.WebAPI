using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DisplayDTOs
{
    public class MerchantResponseDto
    {
        public int? GovernorateId { get; set; }
        public int? CityId { get; set; }
        public string Id { get; set; }
        public string? StoreName { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string userId { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public double? CostPerRefusedOrder { get; set; }

        //public decimal? RefusedOrderPercentage { get; set; }
        public decimal MerchantPayingPercentageForRejectedOrders { get; set; }
        public decimal? SpecialPickupShippingCost { get; set; }
        public List<SpecialPackageDTO> SpecialPackages { get; set; }
        public List<DisplayOrderDTO> orders { get; set; }
        //public UserDto User { get; set; }
    }
}
