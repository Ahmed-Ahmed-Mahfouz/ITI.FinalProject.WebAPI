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
    public class OrderRepository : IOrderRepository
    {
        private readonly ShippingContext _context;

        public OrderRepository(ShippingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetPaginatedElements(int pageNumber, int pageSize)
        {
            return await _context.Orders
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }


        public async Task<int> Count()
        {
            return await _context.Orders.CountAsync();
        }

    }
}
