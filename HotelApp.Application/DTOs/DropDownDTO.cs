using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs
{
	public class DropDownDTO<TKey>
	{
		public int Id { get; set; }
		public TKey DisplayText { get; set; }
	}
}
