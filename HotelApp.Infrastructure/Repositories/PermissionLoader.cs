using HotelApp.Application.Interfaces.IRepositories;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Repositories
{
    public class PermissionLoader : IPermissionLoader
	{
		private readonly IHostEnvironment _env;

		public PermissionLoader(IHostEnvironment env)
		{
			_env = env;
		}

		public List<string> LoadAllPermissions()
		{
			var filePath = GetPermissionFilePath();
			var json = File.ReadAllText(filePath);
			var dict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
			return dict?.SelectMany(x => x.Value).Distinct().ToList() ?? new();
		}

		public Dictionary<string, List<string>> LoadGroupedPermissions()
		{
			var filePath = GetPermissionFilePath();
			var json = File.ReadAllText(filePath);
			return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json)
				   ?? new();
		}

		private string GetPermissionFilePath()
		{
			var path = Path.Combine(_env.ContentRootPath, "permissions.json");
			if (!File.Exists(path))
				throw new FileNotFoundException("Permission file not found.", path);
			return path;
		}
	}

}
