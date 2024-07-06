using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_commerceAPI.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<List<T>> GetAllElements();

        public Task<List<T>> GetAllElements(Expression<Func<T, bool>> filter);

        public Task<List<T>> GetAllElements(params Expression<Func<T, object>>[] includes);

        public Task<List<T>> GetAllElements(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

        public Task<T?> GetElement(Expression<Func<T, bool>> filter);

        public Task<T?> GetElement(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

        public Task<T?> GetElementWithoutTracking(Expression<Func<T, bool>> filter);

        public Task<T?> GetElementWithoutTracking(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

        public bool Add(T element);

        public bool Edit(T element);

        public bool Delete(T element);

        public Task<bool> SaveChanges();
    }
}