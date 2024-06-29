using Application.DTOs.DisplayDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<(List<DisplayOrderDTO>, int)> GetPaginatedOrders(int pageNumber, int pageSize);
    }
}
