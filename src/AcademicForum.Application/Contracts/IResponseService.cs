using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;

namespace AcademicForum.Application.Services
{
    public interface IResponseService
    {
        Task<ApiResponse<List<ResponseDto>>> GetAll();
        Task<ApiResponse<ResponseDto>> GetById(int id);
        Task<ApiResponse<ResponseDto>> Create(CreateResponseDto responseRequest);
        Task<ApiResponse<ResponseDto>> Update(int id, CreateResponseDto responseRequest);
        Task<ApiResponse<ResponseDto>> Delete(int id);
    }
}
