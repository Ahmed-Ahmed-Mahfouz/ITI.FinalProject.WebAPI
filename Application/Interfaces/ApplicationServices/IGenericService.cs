using Application.DTOs.DisplayDTOs;
using Application.DTOs.InsertDTOs;
using Application.DTOs.UpdateDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ApplicationServices
{
    public interface IGenericService<T1, T2, T3, T4> where T1 : class where T2 : class where T3 : class where T4 : class
    {
        public Task<List<T2>> GetAllObjects();

        public Task<List<T2>> GetAllObjects(params Expression<Func<T1, object>>[] includes);

        public Task<T2?> GetObject(Expression<Func<T1, bool>> filter);

        public Task<T2?> GetObject(Expression<Func<T1, bool>> filter, params Expression<Func<T1, object>>[] includes);

        public Task<T2?> GetObjectWithoutTracking(Expression<Func<T1, bool>> filter);

        public Task<T2?> GetObjectWithoutTracking(Expression<Func<T1, bool>> filter, params Expression<Func<T1, object>>[] includes);

        public bool InsertObject(T3 ObjectDTO);

        public bool UpdateObject(T4 ObjectDTO);

        public Task<bool> DeleteObject(int ObjectId);

        public Task<bool> SaveChangesForObject();
    }
}
