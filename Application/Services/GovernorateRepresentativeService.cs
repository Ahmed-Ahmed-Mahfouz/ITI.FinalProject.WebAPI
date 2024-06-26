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
        private readonly IUnitOfWork<GovernorateRepresentatives> unit;

        public GovernorateRepresentativeService(IUnitOfWork<GovernorateRepresentatives> unit)
        {
            this.unit = unit;
        }


    }
}
