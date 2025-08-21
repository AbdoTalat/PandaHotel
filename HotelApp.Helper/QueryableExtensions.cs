using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Helper
{

	public static class QueryableExtensions
	{
		public static IQueryable<T> BranchFilter<T>(this IQueryable<T> query, bool skipFilter = false)
		{
			if (skipFilter)
				return query;

			var branchId = BranchContext.CurrentBranchId;
			if (!branchId.HasValue)
				return query;

			// Check if the entity has a BranchId property of type int
			var propertyInfo = typeof(T).GetProperty("BranchId", BindingFlags.Public | BindingFlags.Instance);
			if (propertyInfo == null || propertyInfo.PropertyType != typeof(int))
				return query;

			// Build EF-compatible expression: e => EF.Property<int>(e, "BranchId") == branchId
			var parameter = Expression.Parameter(typeof(T), "e");
			var efPropertyMethod = typeof(EF)
				.GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)!
				.MakeGenericMethod(typeof(int));

			var branchIdAccess = Expression.Call(efPropertyMethod, parameter, Expression.Constant("BranchId"));
			var branchIdConstant = Expression.Constant(branchId);
			var equality = Expression.Equal(branchIdAccess, branchIdConstant);
			var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

			return query.Where(lambda);
		}




		public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, object filter)
		{
			var filterProps = filter.GetType().GetProperties();

			foreach (var prop in filterProps)
			{
				var value = prop.GetValue(filter);
				if (value == null) continue;

				var entityProp = typeof(T).GetProperty(prop.Name);
				if (entityProp == null) continue;

				var parameter = Expression.Parameter(typeof(T), "e");
				var member = Expression.Property(parameter, entityProp);

				Expression body;

				if (entityProp.PropertyType == typeof(string))
				{
					// String.Contains (case-insensitive)
					var toLower = Expression.Call(member, "ToLower", Type.EmptyTypes);
					var filterValue = Expression.Constant(value.ToString().ToLower());
					body = Expression.Call(toLower, nameof(string.Contains), Type.EmptyTypes, filterValue);
				}
				else
				{
					// Handle nullable types
					var constant = Expression.Constant(value, entityProp.PropertyType);
					body = Expression.Equal(member, constant);
				}

				var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
				query = query.Where(lambda);
			}

			return query;
		}

	}
}
