using Application.Interfaces;
using Application.Interfaces.ApplicationServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GovernorateRepresentativeService
    {
        private readonly IUnitOfWork unit;

        public GovernorateRepresentativeService(IUnitOfWork unit)
        {
            this.unit = unit;
        }


    }
}
