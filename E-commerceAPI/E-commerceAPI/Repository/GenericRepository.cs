using E_commerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_commerceAPI.Repository
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        private readonly ProjectContext db;

        public GenericRepository(ProjectContext db)
        {
            this.db = db;
        }

        public async Task<List<T>> GetAllElements()
        {
            return await db.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllElements(Expression<Func<T,bool>> filter)
        {
            return await db.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<List<T>> GetAllElements(params Expression<Func<T, object>>[] includes)
        {
            var query = db.Set<T>().AsQueryable();

            foreach (var item in includes)
            {
                query = query.Include(item);
            }

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAllElements(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            var query = db.Set<T>().Where(filter);

            foreach (var item in includes)
            {
                query = query.Include(item);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetElement(Expression<Func<T, bool>> filter)
        {
            return await db.Set<T>().FirstOrDefaultAsync(filter);
        }

        public async Task<T?> GetElement(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            var element = db.Set<T>().Where(filter);

            var query = element.AsQueryable();

            foreach (var item in includes)
            {
                query = query.Include(item);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T?> GetElementWithoutTracking(Expression<Func<T, bool>> filter)
        {
            return await db.Set<T>().AsNoTracking().FirstOrDefaultAsync(filter);
        }

        public async Task<T?> GetElementWithoutTracking(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            var element = db.Set<T>().Where(filter);

            var query = element.AsQueryable();

            foreach (var item in includes)
            {
                query = query.Include(item);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public bool Add(T element)
        {
            try
            {
                db.Entry(element).State = EntityState.Added;

                return true;
            }
            catch (Exception) { }

            return false;
        }

        public bool Edit(T element)
        {
            try
            {
                db.Entry(element).State = EntityState.Modified;

                return true;
            }
            catch (Exception) { }

            return false;
        }

        public bool Delete(T element)
        {
            try
            {
                db.Entry(element).State = EntityState.Deleted;

                return true;
            }
            catch (Exception) { }

            return false;
        }

        public async Task<bool> SaveChanges()
        {
            try
            {
                await db.SaveChangesAsync();

                return true;
            }
            catch (Exception) { }

            return false;
        }
    }
}
