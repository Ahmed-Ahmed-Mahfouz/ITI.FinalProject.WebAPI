using AutoMapper.QueryableExtensions;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces.Repositories;
using Application.DTOs.DisplayDTOs;
using Application.DTOs.UpdateDTOs;
using Application.DTOs.InsertDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;


namespace Application.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IGenericRepository<Merchant> _MerchantRepository;
        IUnitOfWork unit;

    
        //private readonly IMerchantRepositories _MerchantRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public MerchantService(
            IUnitOfWork _unit,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _MerchantRepository = _unit.GetGenericRepository<Merchant>();
            unit= _unit;    
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<List<ValidationResult>> InsertObject(MerchantAddDto MerchantAddDto)
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

                _MerchantRepository.Add(_mapper.Map<MerchantAddDto, Merchant>(MerchantAddDto));
                return validationResults;
                
            }
            else
            {
                return validationResults;
               
            }
        }

        public async Task<bool> DeleteObject(string Merchant_id)
        {
            Merchant? Merchant = await _MerchantRepository.GetElement(m=>m.Id==Merchant_id);
            if (Merchant != null)
            {
                _MerchantRepository.Delete(Merchant);
                ApplicationUser? user = await _userManager.FindByEmailAsync(Merchant.Email);
                if (user != null)
                    await _userManager.DeleteAsync(user);
                return true;
            }
            else
                return false;
        }

        public async Task<List<MerchantResponseDto>> GetAllObjects()
        {
            IEnumerable<Merchant>? Merchants = await _MerchantRepository.GetAllElements();
            List<MerchantResponseDto> MerchantsResponse = new List<MerchantResponseDto>();
            foreach (Merchant Merchant in Merchants)
            {
                MerchantsResponse.Add(_mapper.Map<MerchantResponseDto>(Merchant));
            }
            return MerchantsResponse;
        }

        //public async Task<MerchantResponseDto?> GetObject(string id)
        //{
        //    Merchant? Merchant = await _MerchantRepository.GetElement(m=> m.Id == id);
        //    return _mapper.Map<MerchantResponseDto>(Merchant);
        //}

        public async Task<List<ValidationResult>> UpdateObject(MerchantUpdateDto MerchantUpdateDto)
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
                    Merchant? Merchant = await _MerchantRepository.GetElement(m=>m.Id== MerchantUpdateDto.userId);
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
                            await unit.SaveChanges();
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


        //public IQueryable<MerchantResponseDto> GetMerchantsPaginated()
        //{
        //    IQueryable Merchant = _MerchantRepository.GetMerchantsPaginated();
        //    return Merchant.ProjectTo<MerchantResponseDto>(_mapper.ConfigurationProvider);
        //}

        public async Task<List<MerchantResponseDto>> GetFilteredMerchantsAsync(string searchString)
        {
            IEnumerable<Merchant>? Merchants = await _MerchantRepository.GetAllElements(m=> m.UserName == searchString);
            List<MerchantResponseDto> MerchantsResponse = new List<MerchantResponseDto>();
            foreach (Merchant trader in Merchants)
            {
                MerchantsResponse.Add(_mapper.Map<MerchantResponseDto>(Merchants));
            }
            return MerchantsResponse;
        }

        public async Task<string?> GetMerchantIdByEmailAsync(string email, IIdentityUserMapper identityUserMapper)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            Merchant? merchant = await _MerchantRepository.GetElement(m=>m.Id==user.Id);
            if (merchant == null)
                return null;

            return identityUserMapper.MapMerchantToId(merchant); 
        }

        public Task<string?> GetMerchantIdByEmailAsync(string MerchantEmail)
        {
            throw new NotImplementedException();
        }

        
        public async Task<List<MerchantResponseDto>> GetAllObjects(params Expression<Func<Merchant, object>>[] includes)
        {
            IEnumerable<Merchant>? Merchants = await _MerchantRepository.GetAllElements(includes);
            List<MerchantResponseDto> MerchantsResponse = new List<MerchantResponseDto>();
            foreach (Merchant Merchant in Merchants)
            {
                MerchantsResponse.Add(_mapper.Map<MerchantResponseDto>(Merchant));
            }
            return MerchantsResponse;
        }

        

        public async Task<MerchantResponseDto?> GetObject(Expression<Func<Merchant, bool>> filter, params Expression<Func<Merchant, object>>[] includes)
        {
            Merchant? Merchant = await _MerchantRepository.GetElement(filter,includes);
            return _mapper.Map<MerchantResponseDto>(Merchant);
        }

        public async Task<MerchantResponseDto?> GetObjectWithoutTracking(Expression<Func<Merchant, bool>> filter)
        {
            Merchant? Merchant = await _MerchantRepository.GetElementWithoutTracking(filter);
            return _mapper.Map<MerchantResponseDto>(Merchant);
        }

        public async Task<MerchantResponseDto?> GetObjectWithoutTracking(Expression<Func<Merchant, bool>> filter, params Expression<Func<Merchant, object>>[] includes)
        {
            Merchant? Merchant = await _MerchantRepository.GetElementWithoutTracking(filter,includes);
            return _mapper.Map<MerchantResponseDto>(Merchant);
        }

        

        
        

        public Task<bool> SaveChangesForObject()
        {
            throw new NotImplementedException();
        }

        public async Task<MerchantResponseDto?> GetObject(Expression<Func<Merchant, bool>> filter)
        {
            Merchant? Merchant = await _MerchantRepository.GetElement(filter);
            return _mapper.Map<MerchantResponseDto>(Merchant);
        }

        Task<bool> IGenericService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string>.InsertObject(MerchantAddDto ObjectDTO)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string>.UpdateObject(MerchantUpdateDto ObjectDTO)
        {
            throw new NotImplementedException();
        }
    }
}