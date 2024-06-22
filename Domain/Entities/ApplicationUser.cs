using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApplicationUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public int PhoneNo { get; set; }
        public string Branch { get; set; }
    }
}
