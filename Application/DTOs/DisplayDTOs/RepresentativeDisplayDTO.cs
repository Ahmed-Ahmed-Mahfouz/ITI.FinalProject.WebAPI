using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.DisplayDTOs
{
    public class RepresentativeDisplayDTO
    {
        public string DiscountType { get; set; }
        public int CompanyPercetage { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserAddress { get; set; }
        public string UserPhoneNo { get; set; }
        public Status UserStatus { get; set; }
        public int UserBranchId { get; set; }
        public UserType UserType { get; set; }
       // public List<GovernorateRepresentativesDTO> Governorates { get; set; }
    }
}
