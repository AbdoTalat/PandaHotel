using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using HotelApp.Application;
using HotelApp.Infrastructure.DbContext;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using HotelApp.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using HotelApp.Domain.Common;
using System.Security.Claims;
using HotelApp.Application.Services.CurrentUserService;
using HotelApp.Domain;
using HotelApp.Application.Interfaces;

namespace HotelApp.Infrastructure.UnitOfWorks
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _mapperConfig;
        //private readonly ICurrentUserService _currentUserService;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context,
            IConfigurationProvider mapperConfig/*, ICurrentUserService currentUserService*/)
        {
            _context = context;
            _mapperConfig = mapperConfig;
            //_currentUserService = currentUserService;
            _dbSet = _context.Set<T>();
        }

        #region Get Methods
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
                .BranchFilter(SkipBranchFilter);

            if (predicate != null)
                query = query.Where(predicate);

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<TDto>> GetAllAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false) where TDto : class
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
                .BranchFilter(SkipBranchFilter);

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
        public async Task<T?> GetByIdAsync(int id, Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
                .Where(e => EF.Property<int>(e, "Id") == id)
                .BranchFilter(SkipBranchFilter);

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, bool SkipBranchFilter = false)
        {
            IQueryable<T> query = _dbSet
                .AsNoTracking()
                .BranchFilter(SkipBranchFilter);

            if (predicate != null)
                query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<TDto?> FirstOrDefaultAsDtoAsync<TDto>(Expression<Func<T, bool>>? predicate = null, bool skipBranchFilter = false) where TDto : class
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
        {
            var branchId = BranchContext.CurrentBranchId;

            var prop = typeof(T).GetProperty("BranchId");
            if (prop != null && prop.PropertyType == typeof(int?) && branchId.HasValue)
            {
                prop.SetValue(entity, branchId.Value);
            }

            await _dbSet.AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            var branchId = BranchContext.CurrentBranchId;

            foreach (var entity in entities)
            {
                var prop = typeof(T).GetProperty("BranchId");
                if (prop != null && prop.PropertyType == typeof(int?) && branchId.HasValue)
                {
                    prop.SetValue(entity, branchId.Value);
                }
            }

            await _dbSet.AddRangeAsync(entities);
        }
        public void Update(T entity)
            => _dbSet.Update(entity);
        public void UpdateRange(IEnumerable<T> entities)
            => _dbSet.UpdateRange(entities);

        //public async Task BulkUpdateAsync(
        //	Expression<Func<T, bool>> predicate,
        //	Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setProperties,
        //	bool skipBranchFilter = false,
        //	bool skipAuditFields = false)
        //{
        //	IQueryable<T> query = _dbSet.BranchFilter(skipBranchFilter);

        //	if (predicate != null)
        //		query = query.Where(predicate);

        //	if (!skipAuditFields)
        //	{
        //		setProperties = CombineSetProperties(setProperties, s =>
        //			s.SetProperty(e => EF.Property<int>(e, "LastModifiedById"), _currentUserService.UserId)
        //			 .SetProperty(e => EF.Property<DateTime>(e, "LastModifiedDate"), DateTime.UtcNow)
        //		);
        //	}

        //	await query.ExecuteUpdateAsync(setProperties);
        //}

        public void Delete(T entity)
            => _dbSet.Remove(entity);
        public void DeleteRange(IEnumerable<T> entities)
            => _dbSet.RemoveRange(entities);

        public async Task DeleteByIdAsync(int id)
        {
            await _dbSet
                .BranchFilter()
                .Where(e => EF.Property<int>(e, "Id") == id)
                .ExecuteDeleteAsync();
        }
        #endregion

        #region Other Methods
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, bool skipBranchFilter = false)
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
                .BranchFilter(skipBranchFilter);

            return await query.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, bool skipBranchFilter = false)
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
                .BranchFilter(skipBranchFilter);

            if (predicate != null)
                query = query.Where(predicate);

            return await query.CountAsync();
        }

        public async Task<int> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector, bool skipBranchFilter = false)
        {
            IQueryable<T> query = _dbSet.AsNoTracking()
                .BranchFilter(skipBranchFilter);

            if (predicate != null)
                query = query.Where(predicate);

            return await query.SumAsync(selector);
        }

        #endregion


        #region Helper Methods
        //private static Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> CombineSetProperties(
        //    Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> first,
        //    Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> second)
        //{
        //    var param = Expression.Parameter(typeof(SetPropertyCalls<T>), "p");

        //    // Invoke first(p)
        //    var firstBody = Expression.Invoke(first, param);

        //    // Invoke second(first(p))
        //    var secondBody = Expression.Invoke(second, firstBody);

        //    return Expression.Lambda<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>>(secondBody, param);
        //}
        #endregion

    }
}
