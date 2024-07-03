﻿using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.InsertDTOs
{
    public class EmployeeAddDto
    {
        public string Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        [Phone]
        public virtual string PhoneNumber { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public Status Status { get; set; }

       // public bool IsActive { get; set; }

        public ApplicationUser User { get; set; }

        public string role { get; set; }

        //public BranchReadDto? Branch { get; set; }

    }
}