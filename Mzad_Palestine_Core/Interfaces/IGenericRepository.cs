using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByNameAsync(string name);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        Task<bool> ExistsAsync(int id);
        Task DeleteAsync(T entity);  // إضافة دالة الحذف هنا
        Task<int> SaveChangesAsync();
    }
}
