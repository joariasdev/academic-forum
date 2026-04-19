using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;

namespace AcademicForum.Application.Services
{
    public interface IAttendeeRecordService
    {
        Task<ApiResponse<List<AttendeeRecordDto>>> GetAll();
        Task<ApiResponse<AttendeeRecordDto>> GetById(int id);
        Task<ApiResponse<AttendeeRecordDto>> Create(CreateAttendeeRecordDto attendeeRecordRequest);
        Task<ApiResponse<AttendeeRecordDto>> Update(int id, CreateAttendeeRecordDto attendeeRecordRequest);
        Task<ApiResponse<AttendeeRecordDto>> Delete(int id);
    }
}
