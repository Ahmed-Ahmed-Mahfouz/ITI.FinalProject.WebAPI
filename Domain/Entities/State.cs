using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class State
    {
        public int id { get; set; }
        public string name { get; set; }
        public Status status { get; set; }
    }
}
