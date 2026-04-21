using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;

namespace AcademicForum.Application.Services
{
    public interface IDiscussionService
    {
        Task<ApiResponse<List<DiscussionDto>>> GetAll();
        Task<ApiResponse<DiscussionDetailDto>> GetById(int id);
        Task<ApiResponse<DiscussionDto>> Create(CreateDiscussionDto discussionRequest);
        Task<ApiResponse<DiscussionDto>> Update(int id, CreateDiscussionDto discussionRequest);
        Task<ApiResponse<DiscussionDto>> Delete(int id);
    }
}
