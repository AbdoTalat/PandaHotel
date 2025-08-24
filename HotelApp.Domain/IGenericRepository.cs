using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HotelApp.Domain
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false);
        Task<IEnumerable<TDto>> GetAllAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class;
        Task<TDto?> GetByIdAsDtoAsync<TDto>(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class;
        Task<T?> GetByIdAsync(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false);


		Task AddNewAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

		void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

		 Task BulkUpdateAsync(Expression<Func<T, bool>> predicate,
	        Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setProperties,
	        bool skipBranchFilter = false, bool skipAuditFields = false);


		void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task DeleteByIdAsync(int id);


		Task<bool> IsExistsAsync(Expression<Func<T, bool>> predicate, bool skipBranchFilter = false);
		Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false);
        Task<TDto?> FirstOrDefaultAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool skipBranchFilter = false) where TDto : class;

    }

}
