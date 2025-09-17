using HotelApp.Domain.Common.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.DTOs.ProofType
{
	public class ProofTypeDTO
	{
		public int Id { get; set; }

		[MaxLengthEx(40)]
		[RequiredEx]
		public string Name { get; set; }
		public bool IsActive { get; set; }
	}
}
