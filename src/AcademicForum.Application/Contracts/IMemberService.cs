using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;

namespace AcademicForum.Application.Services
{
    public interface IMemberService
    {
        Task<ApiResponse<List<MemberDto>>> GetAll();
        Task<ApiResponse<MemberDto>> GetById(int id);
        Task<ApiResponse<MemberDto>> Create(CreateMemberDto memberRequest);
        Task<ApiResponse<MemberDto>> Update(int id, CreateMemberDto memberRequest);
        Task<ApiResponse<MemberDto>> Delete(int id);
    }
}
