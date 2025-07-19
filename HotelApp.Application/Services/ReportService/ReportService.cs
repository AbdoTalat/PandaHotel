using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Reporting;
using HotelApp.Application.IRepositories;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace HotelApp.Application.Services.ReportService
{
	public class ReportService : IReportService
	{
		private readonly IHostEnvironment _env;
		private const string ReportFolder = "Reports";

		public ReportService(IHostEnvironment env)
		{
			_env = env;
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}

		public byte[] GeneratePdfReport(string reportPath,
								Dictionary<string, string> parameters,
								IEnumerable<object>? data = null,
								string dataSetName = "")
		{

			if (!File.Exists(reportPath))
				throw new FileNotFoundException($"Report file not found at path: {reportPath}");

			var localReport = new LocalReport(reportPath);

			if (data != null && !string.IsNullOrWhiteSpace(dataSetName))
			{
				localReport.AddDataSource(dataSetName, data);
			}

			var result = localReport.Execute(RenderType.Pdf, 1, parameters, string.Empty);
			return result.MainStream;
		}
	}
}
