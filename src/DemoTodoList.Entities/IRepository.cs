using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DemoTodoList.Entities
{
    public interface IRepository<T> where T : class, new()
    {
        Task<T> Get(Guid id);
        Task<T> Get(Expression<Func<T, bool>> predicate);
         Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null);
        Task<bool> Insert(T entity);
        Task<bool> Insert(IEnumerable<T> entities);
        Task<bool> Update(T entity);
        Task<bool> Update(IEnumerable<T> entities);
        Task<bool> Delete(T entity);
        Task<bool> Delete(IEnumerable<T> entities);
    }
}
