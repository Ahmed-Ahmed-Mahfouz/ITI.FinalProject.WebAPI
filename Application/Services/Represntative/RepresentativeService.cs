using Application.DTOs;
using Application.DTOs.InsertDTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Represntative
{
    public class RepresentativeService : IRepresentativeService
    {
        
        public RepresentativeInsertDTO MapToDTO(Representative representative)
        {
            return new RepresentativeInsertDTO
            {
                DiscountType = representative.DiscountType,
                CompanyPercetage = representative.CompanyPercetage,
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
