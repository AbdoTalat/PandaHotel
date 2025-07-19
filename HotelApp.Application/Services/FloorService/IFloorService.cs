using HotelApp.Application.DTOs.Floor;
using HotelApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Application.Services.FloorService
{
    public interface IFloorService
    {
        Task<IEnumerable<GetFloorItemsDTO>> GetFloorItemsAsync();
        Task<IEnumerable<GetAllFloorsDTO>> GetAllFloorsAsync();
        Task<GetFloorByIdDTO?> GetFloorByIdAsync(int Id);
        Task<ServiceResponse<AddFloorDTO>> AddFloorAsync(AddFloorDTO floor);
        Task<EditFloorDTO?> GetFloorToEditByIdAsync(int Id);
        Task<ServiceResponse<EditFloorDTO>> EditFloorAsync(EditFloorDTO floor);
        Task<ServiceResponse<Floor>> DeleteFloorAsync(int Id);
    }
}
