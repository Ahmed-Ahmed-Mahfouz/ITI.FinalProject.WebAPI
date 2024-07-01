using Application.DTOs.UpdateDTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.ApplicationServices;
using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Microsoft.AspNetCore.Identity;
using Application.DTOs;



namespace Application.Services
{
    public class RepresentativeService:IGenericService<Representative,RepresentativeDisplayDTO,RepresentativeInsertDTO,RepresentativeUpdateDTO,string>
    {
        //public RepresentativeDisplayDTO MapToDTO(Representative representative)
        //{
        //    return new RepresentativeDisplayDTO
        //    {
        //        Id= representative.userId,
        //        DiscountType = representative.DiscountType,
        //        CompanyPercetage = representative.CompanyPercetage,
        //        UserFullName = representative.user.FullName,
        //        UserAddress = representative.user.Address,
        //        Email= representative.user.Email,
        //        UserPhoneNo = representative.user.PhoneNo,
        //        UserStatus = representative.user.Status,
        //        UserBranchId = representative.user.BranchId,
        //        UserType = representative.user.UserType,
        //        GovernorateIds = representative.governorates.Select(x => x.governorateId).ToList()

        //    };
        //}

        private readonly IUnitOfWork unit;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Representative> repository;
        private readonly IGenericRepository<GovernorateRepresentatives> GovRepo;

        public RepresentativeService(IUnitOfWork unit,UserManager<ApplicationUser> userManager)
        {
            this.unit = unit;
            this._userManager = userManager;
            this.repository = unit.GetGenericRepository<Representative>();
            this.GovRepo = unit.GetGenericRepository<GovernorateRepresentatives>();
        }

        public async Task<List<RepresentativeDisplayDTO>> GetAllObjects()
        {
            var representatives = await repository.GetAllElements();
            return MapRepresentatives(representatives);
        }
        public async Task<List<RepresentativeDisplayDTO>> GetAllObjects(params Expression<Func<Representative, object>>[] includes)
        {
            var representatives = await repository.GetAllElements(includes);
            return MapRepresentatives(representatives);
        }
        public async Task<RepresentativeDisplayDTO?> GetObject(Expression<Func<Representative, bool>> filter)
        {
            var representative = await repository.GetElement(filter);
            if (representative == null)
            {
                return null;
            }
            return MapRepresentative(representative);
        }
        public async Task<RepresentativeDisplayDTO?> GetObject(Expression<Func<Representative, bool>> filter, params Expression<Func<Representative, object>>[] includes)
        {
            var representative = await repository.GetElement(filter, includes);
            if (representative == null)
            {
                return null;
            }
            return MapRepresentative(representative);
        }
        public async Task<RepresentativeDisplayDTO?> GetObjectWithoutTracking(Expression<Func<Representative, bool>> filter)
        {
            var representative = await repository.GetElementWithoutTracking(filter);
            if (representative == null)
            {
                return null;
            }
            return MapRepresentative(representative);
        }
        public async Task<RepresentativeDisplayDTO?> GetObjectWithoutTracking(Expression<Func<Representative, bool>> filter, params Expression<Func<Representative, object>>[] includes)
        {
            var representative = await repository.GetElementWithoutTracking(filter, includes);
            if (representative == null)
            {
                return null;
            }
            return MapRepresentative(representative);
        }

        public async Task<bool> InsertObject(RepresentativeInsertDTO ObjectDTO)
        {
            var userAdded = new UserDto()
            {
                Email = ObjectDTO.Email,
                Password = ObjectDTO.Password,
                Address = ObjectDTO.UserAddress,
                FullName = ObjectDTO.UserFullName,
                PhoneNo = ObjectDTO.UserPhoneNo,
                BranchId = ObjectDTO.UserBranchId,
                Status = Domain.Enums.Status.Active,
                UserType = Domain.Enums.UserType.Representative,

            };

            var resultUser =await AddUser(userAdded);

            if(resultUser.Message!=null)
            {
                return false;
            }

            var representive = new Representative()
            {
                CompanyPercetage = ObjectDTO.CompanyPercetage,
                DiscountType = ObjectDTO.DiscountType,
                userId = resultUser.UserId,
            };
            var representativeResult=repository.Add(representive);
           var SaveResult=await unit.SaveChanges();
            if (SaveResult == false) return false;
            var govRepresentativeResult = false;
            foreach (var govId in ObjectDTO.GovernorateIds)
            {
                var governrateRepresentative = new GovernorateRepresentatives()
                {
                    representativeId = resultUser.UserId,
                    governorateId = govId
                };

                govRepresentativeResult= GovRepo.Add(governrateRepresentative);
            }
            SaveResult = await unit.SaveChanges();
            return representativeResult && govRepresentativeResult && SaveResult;
        }
        public async Task<bool> UpdateObject(RepresentativeUpdateDTO ObjectDTO)
        {

            var user = await _userManager.FindByIdAsync(ObjectDTO.Id);

            if (user == null)
            {
                return false;
            }

            user.FullName = ObjectDTO.UserFullName;

            user.BranchId = ObjectDTO.UserBranchId;

            var identityResult = await _userManager.ChangePasswordAsync(user, user.PasswordHash, ObjectDTO.Password);

            if (!identityResult.Succeeded)
            {
                return false;
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, ObjectDTO.Email);

            identityResult = await _userManager.ChangeEmailAsync(user, ObjectDTO.Email, token);

            if (!identityResult.Succeeded)
            {
                return false;
            }

            token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, ObjectDTO.UserPhoneNo);

            identityResult = await _userManager.ChangePhoneNumberAsync(user, ObjectDTO.UserPhoneNo, token);

            if (!identityResult.Succeeded)
            {
                return false;
            }

            user.Status = ObjectDTO.UserStatus;

            identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                return false;
            }

            var representative = await repository.GetElement(r => r.userId == ObjectDTO.Id);

            if (representative == null)
            {
                return false;
            }

            representative.CompanyPercetage = ObjectDTO.CompanyPercetage;
            representative.DiscountType = ObjectDTO.DiscountType;
            var result = repository.Edit(representative);

            if (result == false)
            {
                return result;
            }

            var representativeGovernorates = await GovRepo.GetAllElements(gr => gr.representativeId == ObjectDTO.Id);

            foreach (var governorateId in ObjectDTO.GovernorateIds)
            {
                if (representativeGovernorates.FirstOrDefault(rg => rg.governorateId == governorateId) == null)
                {
                    result = GovRepo.Add(new GovernorateRepresentatives()
                    {
                        governorateId = governorateId,
                        representativeId = ObjectDTO.Id
                    });

                    if (result == false)
                    {
                        return result;
                    }
                }
            }

            foreach (var representativeGovernorate in representativeGovernorates)
            {
                if (ObjectDTO.GovernorateIds.FirstOrDefault(gid => gid == representativeGovernorate.governorateId) == 0)
                {
                    result = GovRepo.Delete(representativeGovernorate);

                    if (result == false)
                    {
                        return result;
                    }
                }
            }


            result = await SaveChangesForObject();

            return result;
        }
        public async Task<bool> DeleteObject(string ObjectId)
        {

            var representative = await repository.GetElement(r => r.userId == ObjectId);

            if (representative == null)
            {
                return false;
            }

            var representativeGovernorates = await GovRepo.GetAllElements(rg => rg.representativeId == ObjectId);

            var result = false;

            foreach (var representativeGovernorate in representativeGovernorates)
            {
                result = GovRepo.Delete(representativeGovernorate);
            }

            if (result == false)
            {
                return result;
            }

            result = await SaveChangesForObject();

            if (result == false)
            {
                return result;
            }

            result = repository.Delete(representative);

            result = await SaveChangesForObject();

            if (result == false)
            {
                return result;
            }

            var user = await _userManager.FindByIdAsync(ObjectId);

            if (user == null)
            {
                return false;
            }

            var identityResult = await _userManager.DeleteAsync(user);

            return (identityResult.Succeeded);
        }

        public async Task<bool> SaveChangesForObject()
        {
            var result = await unit.SaveChanges();

            return result;
        }
        private RepresentativeDisplayDTO MapRepresentative(Representative representative)
        {
            var RepresentativeDTO = new RepresentativeDisplayDTO()
            {
                Id = representative.userId,
                DiscountType = representative.DiscountType,
                CompanyPercetage = representative.CompanyPercetage,
                UserFullName = representative.user.FullName,
                UserAddress = representative.user.Address,
                Email = representative.user.Email,
                UserPhoneNo = representative.user.PhoneNo,
                UserStatus = representative.user.Status,
                UserBranchId = representative.user.BranchId,
                UserType = representative.user.UserType,
                GovernorateIds = representative.governorates.Select(x => x.governorateId).ToList()
            };

            return RepresentativeDTO;
        }
        private List<RepresentativeDisplayDTO> MapRepresentatives(List<Representative> representatives)
        {
            var RepresentativesDTO = representatives.Select(r => new RepresentativeDisplayDTO()
            {
                Id = r.userId,
                DiscountType = r.DiscountType,
                CompanyPercetage = r.CompanyPercetage,
                UserFullName = r.user.FullName,
                UserAddress = r.user.Address,
                Email = r.user.Email,
                UserPhoneNo = r.user.PhoneNo,
                UserStatus = r.user.Status,
                UserBranchId = r.user.BranchId,
                UserType = r.user.UserType,
                GovernorateIds = r.governorates.Select(x => x.governorateId).ToList()

            }).ToList();

            return RepresentativesDTO;
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

    }
}
