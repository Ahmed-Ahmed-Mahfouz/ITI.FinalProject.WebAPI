using Application.DTOs;
using Application.DTOs.DisplayDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Represntative
{
    public interface IRepresentativeService
    {
        RepresentativeDisplayDTO MapToDTO(Representative representative);
        
    }
}
