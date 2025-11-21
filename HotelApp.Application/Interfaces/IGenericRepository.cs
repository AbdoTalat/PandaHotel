using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HotelApp.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false, params Expression<Func<T, object>>[] includes);
        Task<List<TDto>> GetAllAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class;
        Task<TDto?> GetByIdAsDtoAsync<TDto>(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class;
        Task<T?> GetByIdAsync(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false, params Expression<Func<T, object>>[] includes);
		Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false);
        Task<TDto?> FirstOrDefaultAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool skipBranchFilter = false) where TDto : class;
		

        Task AddNewAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
		void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

		 //Task BulkUpdateAsync(Expression<Func<T, bool>> predicate,
	  //      Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setProperties,
	  //      bool skipBranchFilter = false, bool skipAuditFields = false);

		void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task DeleteByIdAsync(int id);

		Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, bool skipBranchFilter = false);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, bool skipBranchFilter = false);
        Task<int> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector, bool skipBranchFilter = false);
    }
}
