using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;

namespace AcademicForum.Application.Services
{
    public interface IEventService
    {
        Task<ApiResponse<List<EventDto>>> GetAll();
        Task<ApiResponse<EventDetailDto>> GetById(int id);
        Task<ApiResponse<EventDto>> Create(CreateEventDto eventRequest);
        Task<ApiResponse<EventDto>> Update(int id, CreateEventDto eventRequest);
        Task<ApiResponse<EventDto>> Delete(int id);
    }
}
