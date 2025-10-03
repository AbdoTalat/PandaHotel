using HotelApp.Application.Authorization;
using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Domain.Common;
using HotelApp.Infrastructure.Repositories;


//using HotelApp.UI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace HotelApp.UI.DependencyInjectionExtentions
{
    public static class AuthorizationPoliciesExtension
    {
        public static IServiceCollection AddAuthorizationPoliciesDIAsync(this IServiceCollection services, IPermissionLoader permissionLoader)
		{
			services.AddScoped<IAuthorizationHandler, PermissionHandler>();


			var allPermissions = permissionLoader.LoadAllPermissions();



			// Register each permission as a policy
			services.AddAuthorization(options =>
			{
				foreach (var permission in allPermissions)
				{
					options.AddPolicy(permission, policy =>
						policy.Requirements.Add(new PermissionRequirement(permission)));
				}
			});


            return services;
		}
	}
}
