using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee
    {
        public bool IsActive { get; set; }

        [ForeignKey("user")]
        [Key]
        public string userId { get; set; } = string.Empty;


        public ApplicationUser user { get; set; }
    }
}
