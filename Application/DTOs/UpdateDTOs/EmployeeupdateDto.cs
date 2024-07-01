using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UpdateDTOs
{
    public class EmployeeupdateDto
    {
        public string FullName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        [Phone]
        public virtual string? PhoneNumber { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public string? PasswordHash { get; set; }


        public Status Status { get; set; }

        public bool IsActive { get; set; }
        public string role { get; set; }    

        //public BranchReadDto? Branch { get; set; }

    }
}
