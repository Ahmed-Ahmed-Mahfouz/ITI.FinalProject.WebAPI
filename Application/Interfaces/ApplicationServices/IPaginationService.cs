using Application.DTOs.DisplayDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ApplicationServices
{
    public interface IPaginationService<T1, T2, T3, T4, T5> : IGenericService<T1, T2, T3, T4, T5> where T1 : class where T2 : class where T3 : class where T4 : class
    {
        Task<(List<T2>, int)> GetPaginatedOrders(int pageNumber, int pageSize);
    }
}
