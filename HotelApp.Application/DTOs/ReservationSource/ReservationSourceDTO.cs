using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.ReservationSource
{
	public class ReservationSourceDTO
	{
		public int Id { get; set; }

		[RequiredEx]
		[MaxLengthEx(40)]
		public string Name { get; set; }
		public bool IsActive { get; set; }
	}
}
