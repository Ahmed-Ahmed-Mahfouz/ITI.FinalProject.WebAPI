using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly ShippingContext context;

        public UnitOfWork(ShippingContext context)
        {
            this.context = context;
        }

        //public IGenericRepository<T> GetGenericRepository<T>() where T : class
        //{
        //    return new GenericRepository<T>(context);
        //}

        private IGenericRepository<T> repository;

        public IGenericRepository<T> Repository { get 
            {

                repository ??= new GenericRepository<T>(context);

                return repository;
            } 
        }
    }
}
