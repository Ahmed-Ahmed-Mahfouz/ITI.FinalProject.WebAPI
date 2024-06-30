using AutoMapper.QueryableExtensions;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.DTOs.DisplayDTOs;
using Application.DTOs.UpdateDTOs;
using Application.DTOs.InsertDTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class MerchantService : IGenericRepository<Merchant ,MerchantResponseDto ,MerchantAddDto, MerchantUpdateDto , string >
    {
        private readonly IGenericRepository<Merchant> _MerchantRepository;

    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepositories _MerchantRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public MerchantService(
            IMerchantRepositories MerchantRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _MerchantRepository = MerchantRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<List<ValidationResult>?> AddUserAndMerchant(MerchantAddDto MerchantAddDto)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(MerchantAddDto, null, null);

            // Validate the MerchantAddDto object using DataAnnotations
            Validator.TryValidateObject(MerchantAddDto, context, validationResults, true);

            if (validationResults.Count == 0)
            {
                ApplicationUser? checkUserEmail = await _userManager.FindByEmailAsync(MerchantAddDto.Email);
                if (checkUserEmail is null)
                {
                    ApplicationUser? checkUserName = await _userManager.FindByNameAsync(MerchantAddDto.UserName);
                    if (checkUserName is null)
                    {
                        ApplicationUser user = _mapper.Map<ApplicationUser>(MerchantAddDto);
                        IdentityResult result = await _userManager.CreateAsync(user, MerchantAddDto.PasswordHash);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ValidationResult err = new ValidationResult(error.Description);
                                validationResults.Add(err);
                            }
                            return validationResults;
                        }
                        await _userManager.AddToRoleAsync(user, "Merchant");
                        await _userManager.UpdateAsync(user);
                        ApplicationUser? addedUser = await _userManager.FindByEmailAsync(MerchantAddDto.Email);
                        MerchantAddDto.User = addedUser;
                    }
                    else
                    {
                        validationResults.Add(new ValidationResult("Username is already exist"));
                        return validationResults;
                    }
                }
                else
                {
                    validationResults.Add(new ValidationResult("Email is already exist"));
                    return validationResults;
                }

                await _MerchantRepository.AddMerchantAsync(_mapper.Map<MerchantAddDto, Merchant>(MerchantAddDto));
                return validationResults;
            }
            else
            {
                return validationResults;
            }
        }

        public async Task<bool> DeleteMerchantAsync(int Merchant_id)
        {
            Merchant? Merchant = await _MerchantRepository.GetMerchantByIdAsync(Merchant_id);
            if (Merchant != null)
            {
                await _MerchantRepository.DeleteMerchantAsync(Merchant);
                ApplicationUser? user = await _userManager.FindByEmailAsync(Merchant.Email);
                if (user != null)
                    await _userManager.DeleteAsync(user);
                return true;
            }
            else
                return false;
        }

        public async Task<IEnumerable<MerchantResponseDto>?> GetAllMerchantsAsync()
        {
            IEnumerable<Merchant>? Merchants = await _MerchantRepository.GetAllMerchantsAsync();
            List<MerchantResponseDto> MerchantsResponse = new List<MerchantResponseDto>();
            foreach (Merchant Merchant in Merchants)
            {
                MerchantsResponse.Add(_mapper.Map<MerchantResponseDto>(Merchant));
            }
            return MerchantsResponse;
        }

        public async Task<MerchantResponseDto> GetMerchantByIdAsync(int id)
        {
            Merchant? Merchant = await _MerchantRepository.GetMerchantByIdAsync(id);
            return _mapper.Map<MerchantResponseDto>(Merchant);
        }

        public async Task<List<ValidationResult>?> UpdateMerchantAsync(int MerchantId, MerchantUpdateDto MerchantUpdateDto)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(MerchantUpdateDto, null, null);

            // Validate the MerchantUpdateDto object using DataAnnotations
            Validator.TryValidateObject(MerchantUpdateDto, context, validationResults, true);

            if (validationResults.Count == 0)
            {
                var checkUserName = await _userManager.FindByNameAsync(MerchantUpdateDto.UserName);
                if (checkUserName == null || checkUserName.Id == MerchantUpdateDto.userId)
                {
                    Merchant? Merchant = await _MerchantRepository.GetMerchantByIdAsync(MerchantId);
                    if (Merchant != null)
                    {
                        ApplicationUser? user = await _userManager.FindByEmailAsync(Merchant.Email);
                        if (user != null)
                        {
                            _mapper.Map(MerchantUpdateDto, user);
                            var result = await _userManager.UpdateAsync(user);
                            if (!result.Succeeded)
                            {
                                foreach (var error in result.Errors)
                                {
                                    ValidationResult err = new ValidationResult(error.Description);
                                    validationResults.Add(err);
                                }
                                return validationResults;
                            }

                            _mapper.Map(MerchantUpdateDto, Merchant);
                            await _MerchantRepository.SaveChangesAsync();
                        }
                        else
                        {
                            validationResults.Add(new ValidationResult("User associated with this merchant not found."));
                        }
                    }
                    else
                    {
                        validationResults.Add(new ValidationResult("Merchant not found."));
                    }
                }
                else
                {
                    validationResults.Add(new ValidationResult("Username is already exist."));
                }
                return validationResults;
            }
            else
            {
                return validationResults;
            }
        }


        public IQueryable<MerchantResponseDto> GetMerchantsPaginated()
        {
            IQueryable Merchant = _MerchantRepository.GetMerchantsPaginated();
            return Merchant.ProjectTo<MerchantResponseDto>(_mapper.ConfigurationProvider);
        }

        public async Task<IEnumerable<MerchantResponseDto>> GetFilteredMerchantsAsync(string searchString)
        {
            IEnumerable<Merchant>? Merchants = await _MerchantRepository.GetFilteredMerchantsAsync(searchString);
            List<MerchantResponseDto> MerchantsResponse = new List<MerchantResponseDto>();
            foreach (Merchant trader in Merchants)
            {
                MerchantsResponse.Add(_mapper.Map<MerchantResponseDto>(Merchants));
            }
            return MerchantsResponse;
        }

        public async Task<int?> GetMerchantIdByEmailAsync(string email, IIdentityUserMapper identityUserMapper)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            Merchant? merchant = await _MerchantRepository.GetMerchantByUserIdAsync(user.Id);
            if (merchant == null)
                return null;

            return identityUserMapper.MapMerchantToId(merchant); 
        }

        public Task<int?> GetMerchantIdByEmailAsync(string MerchantEmail)
        {
            throw new NotImplementedException();
        }
    }
}