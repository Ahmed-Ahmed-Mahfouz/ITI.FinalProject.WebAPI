using Application.DTOs;
using Application.DTOs.DisplayDTOs;
using Domain.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.RepresentativeRepo
{
    public interface IRepresentativeRepo
    {
        Task<ResultUser> AddUser(UserDto userDto);
        Task AddRepresentative(Representative representative);
        Task AddGovernRates(GovernorateRepresentatives governorateRepresentatives);
        Task<List<RepresentativeDisplayDTO>> GetAllRepresentative();
    }
}
