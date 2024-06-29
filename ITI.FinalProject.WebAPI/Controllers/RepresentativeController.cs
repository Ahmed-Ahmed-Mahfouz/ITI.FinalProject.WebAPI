using Application.DTOs;
using Application.DTOs.InsertDTOs;
using Application.Services.Represntative;
using Domain.DTO;
using Domain.Entities;
using Domain.Services.RepresentativeRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace ITI.FinalProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepresentativeController : ControllerBase
    {
        private readonly IRepresentativeRepo _representativeRepo;

        public RepresentativeController(IRepresentativeRepo representativeRepo)
        {
            this._representativeRepo = representativeRepo;
        }
        [HttpPost("AddRepresentative")]

        public async Task<ActionResult> AddRepresentative([FromBody] RepresentativeInsertDTO RepresentativeInsertDTO)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Adding User
                    var userAdded = new UserDto()
                    {
                        Email = RepresentativeInsertDTO.Email,
                        Password = RepresentativeInsertDTO.Password,
                        Address = RepresentativeInsertDTO.UserAddress,
                        FullName = RepresentativeInsertDTO.UserFullName,
                        PhoneNo = RepresentativeInsertDTO.UserPhoneNo,
                        BranchId = RepresentativeInsertDTO.UserBranchId,
                        Status = Domain.Enums.Status.Active,
                        UserType = Domain.Enums.UserType.Representative,

                    };
                    var resultUser = await _representativeRepo.AddUser(userAdded);

                    // Adding Representative depend on UserId
                    var representive = new Representative()
                    {
                        CompanyPercetage = RepresentativeInsertDTO.CompanyPercetage,
                        DiscountType = RepresentativeInsertDTO.DiscountType,
                        userId = resultUser.UserId,


                    };
                    await _representativeRepo.AddRepresentative(representive);

                    // Adding Governrates to Representative
                    foreach (var govId in RepresentativeInsertDTO.GovernorateIds)
                    {
                        var governrateRepresentative = new GovernorateRepresentatives()
                        {
                            representativeId = resultUser.UserId,
                            governorateId = govId
                        };

                        await _representativeRepo.AddGovernRates(governrateRepresentative);

                    }
                    transaction.Complete();
                    return Ok(resultUser);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    return BadRequest(new { message = "An error occurred while adding the representative.", details = ex.Message });
                }
            }

        }
    }
}
