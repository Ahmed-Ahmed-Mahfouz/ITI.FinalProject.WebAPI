//using AutoMapper.QueryableExtensions;
//using AutoMapper;
//using Domain.Entities;
//using Microsoft.AspNetCore.Identity;
//using System.ComponentModel.DataAnnotations;
//using Application.Interfaces.Repositories;
//using Application.DTOs.DisplayDTOs;
//using Application.DTOs.UpdateDTOs;
//using Application.DTOs.InsertDTOs;
//using Microsoft.EntityFrameworkCore;
//using System.Linq.Expressions;
//using Application.Interfaces;
//using Application.Interfaces.ApplicationServices;


//namespace Application.Services
//{
//    public class MerchantService : IPaginationService<Merchant , MerchantResponseDto ,MerchantUpdateDto , MerchantAddDto , string>
//    {
//        private readonly IPaginationRepository<Merchant> _MerchantRepository;
//        IUnitOfWork unit;


//        //private readonly IMerchantRepositories _MerchantRepository;
//        private readonly IMapper _mapper;
//        private readonly UserManager<ApplicationUser> _userManager;
//        public MerchantService(
//            IUnitOfWork _unit,
//            IMapper mapper,
//            UserManager<ApplicationUser> userManager)
//        {
//            _MerchantRepository = _unit.GetPaginationRepository<Merchant>();
//            unit = _unit;
//            _mapper = mapper;
//            _userManager = userManager;
//        }
//        public async Task<List<ValidationResult>> InsertObject(MerchantAddDto MerchantAddDto)
//        {
//            var validationResults = new List<ValidationResult>();
//            var context = new ValidationContext(MerchantAddDto, null, null);

//            // Validate the MerchantAddDto object using DataAnnotations
//            Validator.TryValidateObject(MerchantAddDto, context, validationResults, true);

//            if (validationResults.Count == 0)
//            {
//                ApplicationUser? checkUserEmail = await _userManager.FindByEmailAsync(MerchantAddDto.Email);
//                if (checkUserEmail is null)
//                {
//                    ApplicationUser? checkUserName = await _userManager.FindByNameAsync(MerchantAddDto.UserName);
//                    if (checkUserName is null)
//                    {
//                        ApplicationUser user = _mapper.Map<ApplicationUser>(MerchantAddDto);
//                        IdentityResult result = await _userManager.CreateAsync(user, MerchantAddDto.PasswordHash);
//                        if (!result.Succeeded)
//                        {
//                            foreach (var error in result.Errors)
//                            {
//                                ValidationResult err = new ValidationResult(error.Description);
//                                validationResults.Add(err);
//                            }
//                            return validationResults;

//                        }
//                        await _userManager.AddToRoleAsync(user, "Merchant");
//                        await _userManager.UpdateAsync(user);
//                        ApplicationUser? addedUser = await _userManager.FindByEmailAsync(MerchantAddDto.Email);
//                        MerchantAddDto.User = addedUser;

//                    }
//                    else
//                    {
//                        validationResults.Add(new ValidationResult("Username is already exist"));
//                        return validationResults;

//                    }
//                }
//                else
//                {
//                    validationResults.Add(new ValidationResult("Email is already exist"));
//                    return validationResults;

//                }

//                _MerchantRepository.Add(_mapper.Map<MerchantAddDto, Merchant>(MerchantAddDto));
//                return validationResults;

//            }
//            else
//            {
//                return validationResults;

//            }
//        }

//        public async Task<bool> DeleteObject(string Merchant_id)
//        {
//            Merchant? Merchant = await _MerchantRepository.GetElement(m => m.Id == Merchant_id);
//            if (Merchant != null)
//            {
//                _MerchantRepository.Delete(Merchant);
//                ApplicationUser? user = await _userManager.FindByEmailAsync(Merchant.Email);
//                if (user != null)
//                    await _userManager.DeleteAsync(user);
//                return true;
//            }
//            else
//                return false;
//        }

//        public async Task<List<MerchantResponseDto>> GetAllObjects()
//        {
//            IEnumerable<Merchant>? Merchants = await _MerchantRepository.GetAllElements();
//            List<MerchantResponseDto> MerchantsResponse = new List<MerchantResponseDto>();
//            foreach (Merchant Merchant in Merchants)
//            {
//                MerchantsResponse.Add(_mapper.Map<MerchantResponseDto>(Merchant));
//            }
//            return MerchantsResponse;
//        }

//        //public async Task<MerchantResponseDto?> GetObject(string id)
//        //{
//        //    Merchant? Merchant = await _MerchantRepository.GetElement(m=> m.Id == id);
//        //    return _mapper.Map<MerchantResponseDto>(Merchant);
//        //}

//        public async Task<List<ValidationResult>> UpdateObject(MerchantUpdateDto MerchantUpdateDto)
//        {
//            var validationResults = new List<ValidationResult>();
//            var context = new ValidationContext(MerchantUpdateDto, null, null);

//            // Validate the MerchantUpdateDto object using DataAnnotations
//            Validator.TryValidateObject(MerchantUpdateDto, context, validationResults, true);

//            if (validationResults.Count == 0)
//            {
//                var checkUserName = await _userManager.FindByNameAsync(MerchantUpdateDto.UserName);
//                if (checkUserName == null || checkUserName.Id == MerchantUpdateDto.userId)
//                {
//                    Merchant? Merchant = await _MerchantRepository.GetElement(m => m.Id == MerchantUpdateDto.userId);
//                    if (Merchant != null)
//                    {
//                        ApplicationUser? user = await _userManager.FindByEmailAsync(Merchant.Email);
//                        if (user != null)
//                        {
//                            _mapper.Map(MerchantUpdateDto, user);
//                            var result = await _userManager.UpdateAsync(user);
//                            if (!result.Succeeded)
//                            {
//                                foreach (var error in result.Errors)
//                                {
//                                    ValidationResult err = new ValidationResult(error.Description);
//                                    validationResults.Add(err);
//                                }
//                                return validationResults;
//                            }

//                            _mapper.Map(MerchantUpdateDto, Merchant);
//                            await unit.SaveChanges();
//                        }
//                        else
//                        {
//                            validationResults.Add(new ValidationResult("User associated with this merchant not found."));
//                        }
//                    }
//                    else
//                    {
//                        validationResults.Add(new ValidationResult("Merchant not found."));
//                    }
//                }
//                else
//                {
//                    validationResults.Add(new ValidationResult("Username is already exist."));
//                }
//                return validationResults;
//            }
//            else
//            {
//                return validationResults;
//            }
//        }


//        //public IQueryable<MerchantResponseDto> GetMerchantsPaginated()
//        //{
//        //    IQueryable Merchant = _MerchantRepository.GetMerchantsPaginated();
//        //    return Merchant.ProjectTo<MerchantResponseDto>(_mapper.ConfigurationProvider);
//        //}

//        public async Task<List<MerchantResponseDto>> GetFilteredMerchantsAsync(string searchString)
//        {
//            IEnumerable<Merchant>? Merchants = await _MerchantRepository.GetAllElements(m => m.UserName == searchString);
//            List<MerchantResponseDto> MerchantsResponse = new List<MerchantResponseDto>();
//            foreach (Merchant trader in Merchants)
//            {
//                MerchantsResponse.Add(_mapper.Map<MerchantResponseDto>(Merchants));
//            }
//            return MerchantsResponse;
//        }

//        public async Task<string?> GetMerchantIdByEmailAsync(string email, IIdentityUserMapper identityUserMapper)
//        {
//            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
//            if (user == null)
//                return null;

//            Merchant? merchant = await _MerchantRepository.GetElement(m => m.Id == user.Id);
//            if (merchant == null)
//                return null;

//            return identityUserMapper.MapMerchantToId(merchant);
//        }

//        public Task<string?> GetMerchantIdByEmailAsync(string MerchantEmail)
//        {
//            throw new NotImplementedException();
//        }


//        public async Task<List<MerchantResponseDto>> GetAllObjects(params Expression<Func<Merchant, object>>[] includes)
//        {
//            IEnumerable<Merchant>? Merchants = await _MerchantRepository.GetAllElements(includes);
//            List<MerchantResponseDto> MerchantsResponse = new List<MerchantResponseDto>();
//            foreach (Merchant Merchant in Merchants)
//            {
//                MerchantsResponse.Add(_mapper.Map<MerchantResponseDto>(Merchant));
//            }
//            return MerchantsResponse;
//        }



//        public async Task<MerchantResponseDto?> GetObject(Expression<Func<Merchant, bool>> filter, params Expression<Func<Merchant, object>>[] includes)
//        {
//            Merchant? Merchant = await _MerchantRepository.GetElement(filter, includes);
//            return _mapper.Map<MerchantResponseDto>(Merchant);
//        }

//        public async Task<MerchantResponseDto?> GetObjectWithoutTracking(Expression<Func<Merchant, bool>> filter)
//        {
//            Merchant? Merchant = await _MerchantRepository.GetElementWithoutTracking(filter);
//            return _mapper.Map<MerchantResponseDto>(Merchant);
//        }

//        public async Task<MerchantResponseDto?> GetObjectWithoutTracking(Expression<Func<Merchant, bool>> filter, params Expression<Func<Merchant, object>>[] includes)
//        {
//            Merchant? Merchant = await _MerchantRepository.GetElementWithoutTracking(filter, includes);
//            return _mapper.Map<MerchantResponseDto>(Merchant);
//        }

//        public Task<bool> SaveChangesForObject()
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<MerchantResponseDto?> GetObject(Expression<Func<Merchant, bool>> filter)
//        {
//            Merchant? Merchant = await _MerchantRepository.GetElement(filter);
//            return _mapper.Map<MerchantResponseDto>(Merchant);
//        }

//        public Task<(List<MerchantResponseDto>, int)> GetPaginatedOrders(int pageNumber, int pageSize)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<ModificationResultDTO> InsertObject(MerchantUpdateDto ObjectDTO)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<ModificationResultDTO> UpdateObject(MerchantAddDto ObjectDTO)
//        {
//            throw new NotImplementedException();
//        }

//        Task<ModificationResultDTO> IGenericService<Merchant, MerchantResponseDto, MerchantUpdateDto, MerchantAddDto, string>.DeleteObject(string ObjectId)
//        {
//            throw new NotImplementedException();
//        }

//        //Task<bool> IGenericService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string>.InsertObject(MerchantAddDto ObjectDTO)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        //Task<bool> IGenericService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string>.UpdateObject(MerchantUpdateDto ObjectDTO)
//        //{
//        //    throw new NotImplementedException();
//        //}
//    }
//}

using Application.DTOs;
using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MerchantService : IPaginationService<Merchant, MerchantResponseDto,  MerchantAddDto, MerchantUpdateDto,string>
    {
        
        
        private readonly IUnitOfWork unit;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaginationRepository<Merchant> repository;
        private readonly IMapper _mapper;

        public MerchantService(IMapper mapper,IUnitOfWork unit, UserManager<ApplicationUser> userManager)
        {
            this.unit = unit;
            this._userManager = userManager;
            this.repository = unit.GetPaginationRepository<Merchant>();;
            _mapper = mapper;
        }

        public async Task<List<MerchantResponseDto>> GetAllObjects()
        {
            var merchants = await repository.GetAllElements();
            List<MerchantResponseDto> result = new List<MerchantResponseDto>();
            foreach (var merchant in merchants)
                result.Add( MapMerchants(merchant));
            return  result;
         }
        public async Task<List<MerchantResponseDto>> GetAllObjects(params Expression<Func<Merchant, object>>[] includes)
        {
            var merchants = await repository.GetAllElements(includes);
            List<MerchantResponseDto> result = new List<MerchantResponseDto>();
            foreach (var merchant in merchants)
                result.Add(MapMerchants(merchant));
            return result;
        }
        public async Task<MerchantResponseDto?> GetObject(Expression<Func<Merchant, bool>> filter)
        {
            var merchant = await repository.GetElement(filter);
            if (merchant == null)
            {
                return null;
            }
            return MapMerchants(merchant);
        }
        public async Task<MerchantResponseDto?> GetObject(Expression<Func<Merchant, bool>> filter, params Expression<Func<Merchant, object>>[] includes)
        {
            var representative = await repository.GetElement(filter, includes);
            if (representative == null)
            {
                return null;
            }
            return MapMerchants(representative);
        }
        public async Task<MerchantResponseDto?> GetObjectWithoutTracking(Expression<Func<Merchant, bool>> filter)
        {
            var merhcant = await repository.GetElementWithoutTracking(filter);
            if (merhcant == null)
            {
                return null;
            }
            return MapMerchants(merhcant);
        }
        public async Task<MerchantResponseDto?> GetObjectWithoutTracking(Expression<Func<Merchant, bool>> filter, params Expression<Func<Merchant, object>>[] includes)
        {
            var merhcant = await repository.GetElementWithoutTracking(filter, includes);
            if (merhcant == null)
            {
                return null;
            }
            return MapMerchants(merhcant);
        }

        public async Task<ModificationResultDTO> InsertObject(MerchantAddDto ObjectDTO)
        {
            var userAdded = new UserDto()
            {
                Email = ObjectDTO.Email,
                Password = ObjectDTO.PasswordHash,
                Address = ObjectDTO.Address,
                FullName = ObjectDTO.UserName,
                PhoneNo = ObjectDTO.PhoneNumber,
                Status = Domain.Enums.Status.Active,
                UserType = Domain.Enums.UserType.Merchant,

            };

            var resultUser = await AddUser(userAdded);

            if (resultUser.Message != null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = resultUser.Message
                };
            }

            var merchant = new Merchant()
            {
                PhoneNumber = ObjectDTO.PhoneNumber,
                CityId = ObjectDTO.city.id,
                Email=ObjectDTO.Email,
                PasswordHash = ObjectDTO.PasswordHash,
                Address = ObjectDTO.Address,
                Id= ObjectDTO.Id,
                GovernorateId = ObjectDTO.governorate.id,
                PhoneNo=ObjectDTO.PhoneNumber,
                UserType = Domain.Enums.UserType.Merchant,
            };

            var merchantResult = repository.Add(merchant);

            if (merchantResult == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error inserting the representative"
                };
            }

            var saveResult = await unit.SaveChanges();

            if (saveResult == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error saving the changes"
                };
            }


            if (saveResult == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error saving the changes"
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = true
            };
        }
        public async Task<ModificationResultDTO> UpdateMerchant(MerchantUpdateDto ObjectDTO)
        {
            // Find the user associated with the merchant
            var user = await _userManager.FindByIdAsync(ObjectDTO.Id);

            if (user == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "User doesn't exist in the db"
                };
            }

            // Update user information
            user.FullName = ObjectDTO.UserName;

            var identityResult = await _userManager.ChangePasswordAsync(user, user.PasswordHash, ObjectDTO.PasswordHash);

            if (!identityResult.Succeeded)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error changing user password"
                };
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, ObjectDTO.Email);

            identityResult = await _userManager.ChangeEmailAsync(user, ObjectDTO.Email, token);

            if (!identityResult.Succeeded)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error changing user email"
                };
            }

            token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, ObjectDTO.PhoneNumber);

            identityResult = await _userManager.ChangePhoneNumberAsync(user, ObjectDTO.PhoneNumber, token);

            if (!identityResult.Succeeded)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error changing user phone number"
                };
            }

            user.Status = ObjectDTO.User.Status;
            identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error updating the user"
                };
            }

            // Find the merchant
            var merchant = await repository.GetElement(m => m.Id == ObjectDTO.Id);

            if (merchant == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Merchant doesn't exist in the db"
                };
            }

            // Update merchant information
            merchant.StoreName = ObjectDTO.StoreName;
            merchant.GovernorateId = ObjectDTO.governerateID;
            merchant.CityId = ObjectDTO.cityID; // Assuming you have CityId in the MerchantUpdateDto
            merchant.Address = ObjectDTO.Address; // Assuming you have Address in the MerchantUpdateDto
            merchant.PhoneNumber = ObjectDTO.PhoneNumber; // Assuming you have PhoneNumber in the MerchantUpdateDto

            var result = repository.Edit(merchant);

            if (!result)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error updating the merchant"
                };
            }

            // Save changes
            result = await SaveChangesForObject();

            if (!result)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error saving changes"
                };
            }

            return new ModificationResultDTO()
            {
                Succeeded = true
            };
        }
        public async Task<ModificationResultDTO> DeleteObject(string ObjectId)
        {

            var merchant = await repository.GetElement(r => r.Id == ObjectId);

            if (merchant == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Merchant doesn't exist in the db"
                };
            }

            var merchants = await repository.GetAllElements(rg => rg.Id == ObjectId);

            var result = false;

            foreach (var mer in merchants)
            {
                result = repository.Delete(mer);

                if (result == false)
                {
                    return new ModificationResultDTO()
                    {
                        Succeeded = false,
                        Message = "Error deleting the governorate representative"
                    };
                }
            }


            result = await SaveChangesForObject();

            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error saving changes"
                };
            }

            result = repository.Delete(merchant);
            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error deleting the Merchant"
                };
            }

            result = await SaveChangesForObject();

            if (result == false)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "Error saving changes"
                };
            }

            var user = await _userManager.FindByIdAsync(ObjectId);

            if (user == null)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = false,
                    Message = "User doesn't exist in the db"
                };
            }

            var identityResult = await _userManager.DeleteAsync(user);

            if (identityResult.Succeeded)
            {
                return new ModificationResultDTO()
                {
                    Succeeded = true
                };
            }


            return new ModificationResultDTO()
            {
                Succeeded = false,
                Message = "Error deleting user"
            };
        }

        public async Task<bool> SaveChangesForObject()
        {
            var result = await unit.SaveChanges();

            return result;
        }
        private MerchantResponseDto MapMerchants(Merchant merchant)
        {
            var ordersAfterMapper = _mapper.Map<List<DisplayOrderDTO>>(merchant.orders);
            var packagesAfterMapper = _mapper.Map<List<SpecialPackageDTO>>(merchant.SpecialPackages);
            var MerchantResponseDto = new MerchantResponseDto()
            {
                Id = merchant.Id,
                Address = merchant.Address,
                CityId=merchant.CityId,
                Email=merchant.Email,
                GovernorateId= merchant.GovernorateId,
                MerchantPayingPercentageForRejectedOrders= merchant.MerchantPayingPercentageForRejectedOrders,
                orders= ordersAfterMapper,
                PasswordHash = merchant.PasswordHash,
                PhoneNumber= merchant.PhoneNumber,
                SpecialPackages= packagesAfterMapper,
                SpecialPickupShippingCost= merchant.SpecialPickupShippingCost,
                StoreName= merchant.StoreName,
                UserName=merchant.UserName
            };

            return MerchantResponseDto;
        }
        private List<MerchantResponseDto> MapMerchants(List<Merchant> merchants)
        {
            var MerchantResponseDtos = merchants.Select(merchant => new MerchantResponseDto()
            {
                Id = merchant.Id,
                Address = merchant.Address,
                CityId = merchant.CityId,
                Email = merchant.Email,
                GovernorateId = merchant.GovernorateId,
                MerchantPayingPercentageForRejectedOrders = merchant.MerchantPayingPercentageForRejectedOrders,
                PasswordHash = merchant.PasswordHash,
                PhoneNumber = merchant.PhoneNumber,
                orders = _mapper.Map<List<DisplayOrderDTO>>(merchant.orders),
                SpecialPackages = _mapper.Map<List<SpecialPackageDTO>>(merchant.SpecialPackages),
                SpecialPickupShippingCost = merchant.SpecialPickupShippingCost,
                StoreName = merchant.StoreName,
                UserName = merchant.UserName
            }).ToList();

            return MerchantResponseDtos;
        }
        public async Task<ResultUser> AddUser(UserDto userDto)
        {
            if (await _userManager.FindByEmailAsync(userDto.Email) != null)
                return new ResultUser { Message = "Email is Already registered!" };

            if (await _userManager.FindByNameAsync(userDto.FullName.Trim().Replace(' ', '_')) != null)
                return new ResultUser { Message = "UserName is Already registered!" };

            var user = new ApplicationUser
            {
                UserName = userDto.FullName.Trim().Replace(' ', '_'), //userDto.Email,
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

        public Task<(List<RepresentativeDisplayDTO>, int)> GetPaginatedOrders(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }


        public Task<ModificationResultDTO> InsertObject(MerchantUpdateDto ObjectDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ModificationResultDTO> UpdateObject(MerchantAddDto ObjectDTO)
        {
            throw new NotImplementedException();
        }

        Task<(List<MerchantResponseDto>, int)> IPaginationService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string>.GetPaginatedOrders(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ModificationResultDTO> UpdateObject(MerchantUpdateDto ObjectDTO)
        {
            throw new NotImplementedException();
        }
        //Task<bool> IGenericService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string>.InsertObject(MerchantAddDto ObjectDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<bool> IGenericService<Merchant, MerchantResponseDto, MerchantAddDto, MerchantUpdateDto, string>.UpdateObject(MerchantUpdateDto ObjectDTO)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

