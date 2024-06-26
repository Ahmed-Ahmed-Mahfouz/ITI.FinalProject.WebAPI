﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UpdateDTOs
{
    public class BranchUpdateDTO
    {
        public string name { get; set; }
        public Status status { get; set; }
        public DateTime addingDate { get; set; }
        public int cityId { get; set; }
    }
}