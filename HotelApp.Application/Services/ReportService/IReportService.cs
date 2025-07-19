using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.ReportService
{
	public interface IReportService
	{
		byte[] GeneratePdfReport(string reportPath, Dictionary<string, string> parameters,
			IEnumerable<object>? data = null, string dataSetName = "");
	}
}
