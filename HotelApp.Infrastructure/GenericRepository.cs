using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using HotelApp.Domain;
using HotelApp.Infrastructure.DbContext;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using HotelApp.Helper;
using Microsoft.AspNetCore.Http;

namespace HotelApp.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;
		private readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context,
            IConfigurationProvider mapperConfig)
        {
            _context = context;
            _mapperConfig = mapperConfig;
			_dbSet = _context.Set<T>();
        }

		#region Get Methods
		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false)
        {
			IQueryable<T> query = _dbSet.AsNoTracking()
				.BranchFilter(SkipBranchFilter);

            var a = query;

			if (predicate != null)
				query = query.Where(predicate);

			return await query.ToListAsync();
		}
        public async Task<IEnumerable<TDto>> GetAllAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
				.BranchFilter(SkipBranchFilter);

            var s = query;

			if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ProjectTo<TDto>(_mapperConfig).ToListAsync();
        }
		public async Task<TDto?> GetByIdAsDtoAsync<TDto>(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class
		{
			IQueryable<T> query = _dbSet.AsNoTracking()
				.Where(e => EF.Property<int>(e, "Id") == id)
				.BranchFilter(SkipBranchFilter);

			if (predicate != null)
			{
				query = query.Where(predicate);
			}

			return await query
				.ProjectTo<TDto>(_mapperConfig)
				.FirstOrDefaultAsync();
		}
		public async Task<T?> GetByIdAsync(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false)
		{
            IQueryable<T> query = _dbSet.AsNoTracking()
                .Where(e => EF.Property<int>(e, "Id") == id)
                .BranchFilter(SkipBranchFilter);
                
            if (predicate != null)
			{
				query = query.Where(predicate);
			}

			return await query.FirstOrDefaultAsync();
		}
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false)
        {
            IQueryable<T> query = _dbSet
                .AsNoTracking()
                .BranchFilter(SkipBranchFilter); ;

            if (predicate != null)
                query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<TDto?> FirstOrDefaultAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null,bool skipBranchFilter = false)where TDto : class
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
                .BranchFilter(skipBranchFilter);

            if (predicate != null)
                query = query.Where(predicate);

            return await query
                .ProjectTo<TDto>(_mapperConfig)
                .FirstOrDefaultAsync();
        }
		#endregion

		#region Add & Edit & Delete
		public async Task AddNewAsync(T entity)
            => await _dbSet.AddAsync(entity);
        public async Task AddRangeAsync(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);
        public void Update(T entity)
            => _dbSet.Update(entity);
        public void UpdateRange(IEnumerable<T> entities)
            => _dbSet.UpdateRange(entities);
		public void Delete(T entity)
            => _dbSet.Remove(entity);
        public void DeleteRange(IEnumerable<T> entities)
            => _dbSet.RemoveRange(entities);
		#endregion

		#region Other Methods
		public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> predicate)
            =>await _dbSet.AnyAsync(predicate);
        
        public IQueryable<T> GetAllIQueryable()
        {
            return _dbSet;
        }
		#endregion

	}
}
