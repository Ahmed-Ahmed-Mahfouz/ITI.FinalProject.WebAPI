using Application.DTOs;
using Application.DTOs.DisplayDTOs;
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
        
        public RepresentativeDisplayDTO MapToDTO(Representative representative)
        {
            return new RepresentativeDisplayDTO
            {
                Id= representative.userId,
                DiscountType = representative.DiscountType,
                CompanyPercetage = representative.CompanyPercetage,
                UserFullName = representative.user.FullName,
                UserAddress = representative.user.Address,
                Email= representative.user.Email,
                UserPhoneNo = representative.user.PhoneNo,
                UserStatus = representative.user.Status,
                UserBranchId = representative.user.BranchId,
                UserType = representative.user.UserType,
                GovernorateIds = representative.governorates.Select(x => x.governorateId).ToList()
                
            };
        }
       

    } 
}
