using Application.DTOs;
using Application.DTOs.DisplayDTOs;
using Application.Services.Represntative;
using Domain.DTO;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.RepresentativeRepo
{
    public class RepresentativeRepo: IRepresentativeRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ShippingContext _context;
        private readonly IRepresentativeService _representativeService;

        public RepresentativeRepo(UserManager<ApplicationUser> userManager,ShippingContext context,IRepresentativeService representativeService)
        {
            this._userManager = userManager;
            this._context = context;
            this._representativeService = representativeService;
        }
        public async Task<ResultUser> AddUser(UserDto userDto)
        {
            if (await _userManager.FindByEmailAsync(userDto.Email) != null)
                return new ResultUser { Message = "Email is Already registered!" };
            var user = new ApplicationUser
            {

                UserName = userDto.Email,
                Email = userDto.Email,
                FullName = userDto.FullName,
                UserType = userDto.UserType,
                Status = userDto.Status,
                PhoneNo = userDto.PhoneNo,
                Address = userDto.Address,
                BranchId = userDto.BranchId
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }

                return new ResultUser { Message = errors };
            }

            return new ResultUser
            {
                Email = user.Email,
                IsAuthenticated = true,
                Username = user.Email,
                UserId = user.Id

            };

        }
        public async Task AddRepresentative(Representative representative)
        {
           await _context.Representatives.AddAsync(representative);
            await _context.SaveChangesAsync();

        }
        public async Task AddGovernRates(GovernorateRepresentatives governorateRepresentatives)
        {
           await _context.GovernorateRepresentatives.AddAsync(governorateRepresentatives);
            await _context.SaveChangesAsync();

        }

        public async Task<List<RepresentativeDisplayDTO>> GetAllRepresentative()
        {
           var representatives= await _context.Representatives.Include(x=>x.governorates).Include(x=>x.user).ToListAsync();
            var representativesDtos = new List<RepresentativeDisplayDTO>();
            foreach (var item in representatives)
            {
               representativesDtos.Add(_representativeService.MapToDTO(item));
            }
            return representativesDtos;


        }
    }
}
