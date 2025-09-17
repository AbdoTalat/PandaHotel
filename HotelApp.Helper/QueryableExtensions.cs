using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

			//Check if the entity has a BranchId property of type int
		   var propertyInfo = typeof(T).GetProperty("BranchId", BindingFlags.Public | BindingFlags.Instance);
			if (propertyInfo == null || propertyInfo.PropertyType != typeof(int?))
				return query;

			//Build EF-compatible expression: e => EF.Property<int>(e, "BranchId") == branchId
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

	}
}
