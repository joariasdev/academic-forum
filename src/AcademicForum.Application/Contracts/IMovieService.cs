using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;

namespace AcademicForum.Application.Services
{
    public interface IMovieService
    {
        Task<ApiResponse<List<MovieDto>>> GetAll();
        Task<ApiResponse<MovieDetailDto>> GetById(int id);
        Task<ApiResponse<MovieDto>> Create(CreateMovieDto request);
        Task<ApiResponse<MovieDto>> Update(int id, CreateMovieDto request);
        Task<ApiResponse<MovieDto>> Delete(int id);
    }
}
