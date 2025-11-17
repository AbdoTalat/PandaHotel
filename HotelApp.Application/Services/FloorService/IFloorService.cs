using HotelApp.Application.DTOs;
using HotelApp.Application.DTOs.Floor;
using HotelApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.FloorService
{
    public interface IFloorService
    {
        Task<IEnumerable<SelectListItem>> GetFloorsDropDownAsync();
        Task<IEnumerable<GetAllFloorsDTO>> GetAllFloorsAsync();
        Task<GetFloorByIdDTO?> GetFloorByIdAsync(int Id);
        Task<ServiceResponse<FloorDTO>> AddFloorAsync(FloorDTO floor);
        Task<FloorDTO?> GetFloorToEditByIdAsync(int Id);
        Task<ServiceResponse<FloorDTO>> EditFloorAsync(FloorDTO floor);
        Task<ServiceResponse<Floor>> DeleteFloorAsync(int Id);
    }
}
