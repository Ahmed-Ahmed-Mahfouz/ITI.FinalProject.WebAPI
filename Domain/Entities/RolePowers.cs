using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RolePowers
    {
        
        [ForeignKey("ApplicationRoles")]
        public string RoleId { get; set; }
        public PowerTypes Power { get; set; }
        public ApplicationRoles ApplicationRoles { get; set; }
    }

}
