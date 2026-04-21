using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;

namespace AcademicForum.Application.Services
{
    public interface IVenueService
    {
        Task<ApiResponse<List<VenueDto>>> GetAll();
        Task<ApiResponse<VenueDetailDto>> GetById(int id);
        Task<ApiResponse<VenueDto>> Create(CreateVenueDto venueRequest);
        Task<ApiResponse<VenueDto>> Update(int id, CreateVenueDto venueRequest);
        Task<ApiResponse<VenueDto>> Delete(int id);
    }
}
