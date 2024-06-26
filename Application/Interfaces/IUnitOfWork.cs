using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitOfWork //where T : class
    {
        public IGenericRepository<T> GetGenericRepository<T>() where T : class;
        //public IGenericRepository<T> Repository { get; }

        public Task<bool> SaveChanges();
    }
}
