using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class PaginationRepository<T> : GenericRepository<T>, IPaginationRepository<T> where T : class
    {
        private readonly ShippingContext _context;

        public PaginationRepository(ShippingContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetPaginatedElements(int pageNumber, int pageSize)
        {
            return await _context.Set<T>()
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }


        public async Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }

    }
}
