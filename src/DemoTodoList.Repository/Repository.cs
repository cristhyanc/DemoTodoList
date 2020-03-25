using DemoTodoList.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DemoTodoList.Repository
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class, new()
    {


        private DBContext context;
        private SQLiteAsyncConnection dbConnection
        {
            get
            {
                return DBContext.Database;
            }
        }
        protected static object collisionLock = new object();


        public Repository(DBContext _context)
        {
            context = _context; 
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
           
            return await dbConnection.Table<T>().ToListAsync().ConfigureAwait(false);
        }


        public virtual async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            var query = dbConnection.Table<T>();
            if (predicate != null)
                query = query.Where(predicate);

             return await query.ToListAsync().ConfigureAwait(false);

        }

        public virtual async Task<T> Get(Guid id)
        {

            return await dbConnection.FindAsync<T>(id);
        }

        public virtual async Task<T> Get(Expression<Func<T, bool>> predicate)
        {

            return await dbConnection.FindAsync<T>(predicate);
        }

        public virtual async  Task<bool>  Insert(T entity)
        {
            if (await dbConnection.InsertAsync(entity) > 0)
            {
                return true;
            }
            return false;
        }

        public virtual async Task<bool> Insert(IEnumerable<T> entities)
        {
            if (await dbConnection.InsertAllAsync(entities) > 0)
            {
                return true;
            }
            return false;
        }

        public virtual async Task<bool> Update(T entity)
        {
            if (await dbConnection.UpdateAsync(entity, entity.GetType()) > 0)
            {
                return true;
            }
            return false;
        }

        public virtual async Task<bool> Update(IEnumerable<T> entities)
        {
            if (await dbConnection.UpdateAllAsync(entities) > 0)
            {
                return true;
            }
            return false;
        }

        public virtual async Task<bool> Delete(T entity)
        {

            if (await dbConnection.DeleteAsync(entity) > 0)
            {
                return true;
            }
            return false;
        }
        public virtual async Task<bool> Delete(IEnumerable<T> entities)
        {
            bool result = false;
            foreach (var item in entities)
            {
                result =await Delete(item);
            }
            return result;
        }

      

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
