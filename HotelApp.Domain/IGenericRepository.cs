using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelApp.Domain
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false);
        Task<IEnumerable<TDto>> GetAllAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class;


        public Task<TDto?> GetByIdAsDtoAsync<TDto>(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class;

        public Task<T?> GetByIdAsync(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false);




        Task AddNewAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

		void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);


		void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        Task<bool> IsExistsAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false);
        Task<TDto?> FirstOrDefaultAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool skipBranchFilter = false) where TDto : class;

    }

}
