using Application.DTOs.DisplayDTOs;
using Domain.Entities;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Represntative
{
    public class RepresentativeService : IRepresentativeService
    {
        public RepresentativeDisplayDTO MapToDTO(Representative representative)
        {
            return new RepresentativeDisplayDTO
            {
                DiscountType = representative.DiscountType,
                CompanyPercetage = representative.CompanyPercetage,
                UserId = representative.userId,
                UserFullName = representative.user.FullName,
                UserAddress = representative.user.Address,
                UserPhoneNo = representative.user.PhoneNo,
                UserStatus = representative.user.Status,
                UserBranchId = representative.user.BranchId,
                UserType = representative.user.UserType,
                //Governorates = representative.governorates?.Select(g => new GovernorateRepresentativesDTO
                //{
                //    // Map properties from GovernorateRepresentatives to GovernorateRepresentativesDTO
                //}).ToList()
            };
        }
       
    } 
}
