using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IPaginationRepository<T> : IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetPaginatedElements(int pageNumber, int pageSize, Expression<Func<T, bool>> filter);
        Task<int> Count();
        Task<int> Pages(int pageSize);
    }
}
