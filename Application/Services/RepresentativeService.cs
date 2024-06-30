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



namespace Application.Services
{
    public class RepresentativeService:IGenericService<Representative,RepresentativeDisplayDTO,RepresentativeInsertDTO,ReoresentativeUpdateDTO,string>
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
        public Task<bool> UpdateObject(ReoresentativeUpdateDTO ObjectDTO)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> SaveChangesForObject()
        {
            var result = await unit.SaveChanges();

            return result;
        }
        public Task<bool> DeleteObject(string ObjectId)
        {
            throw new NotImplementedException();
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

    }
}
