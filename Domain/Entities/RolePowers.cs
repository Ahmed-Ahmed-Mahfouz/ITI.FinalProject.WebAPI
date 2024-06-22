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
    internal class RolePowers
    {
        [Key]
        //[ForeignKey()]
        public Guid RoleId { get; set; }
        public PowerTypes Power { get; set; }
    }

}
