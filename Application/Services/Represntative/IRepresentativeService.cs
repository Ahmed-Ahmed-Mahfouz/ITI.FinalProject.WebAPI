﻿using Application.DTOs;
using Application.DTOs.InsertDTOs;
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
        RepresentativeInsertDTO MapToDTO(Representative representative);
        
    }
}
