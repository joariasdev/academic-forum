using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;
using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using FluentValidation;
using Mapster;

namespace AcademicForum.Application.Services;

public class AttendeeRecordService : IAttendeeRecordService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateAttendeeRecordDto> _validator;

    public AttendeeRecordService(IUnitOfWork unitOfWork, IValidator<CreateAttendeeRecordDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ApiResponse<List<AttendeeRecordDto>>> GetAll()
    {
        var records = await _unitOfWork.AttendeeRecords.GetAllAsync();
        return ApiResponse<List<AttendeeRecordDto>>.SuccessResponse(records.Adapt<List<AttendeeRecordDto>>());
    }

    public async Task<ApiResponse<AttendeeRecordDto>> GetById(int id)
    {
        var record = await _unitOfWork.AttendeeRecords.GetByIdAsync(id);

        return record is null
            ? ApiResponse<AttendeeRecordDto>.FailureResponse($"AttendeeRecord with id {id} not found.", 404)
            : ApiResponse<AttendeeRecordDto>.SuccessResponse(record.Adapt<AttendeeRecordDto>());
    }

    public async Task<ApiResponse<AttendeeRecordDto>> Create(CreateAttendeeRecordDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<AttendeeRecordDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var eventExists = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (eventExists is null)
            return ApiResponse<AttendeeRecordDto>.FailureResponse($"Event with id {request.EventId} not found.", 404);

        var memberExists = await _unitOfWork.Members.GetByIdAsync(request.MemberId);
        if (memberExists is null)
            return ApiResponse<AttendeeRecordDto>.FailureResponse($"Member with id {request.MemberId} not found.", 404);

        var record = request.Adapt<AttendeeRecord>();
        await _unitOfWork.AttendeeRecords.AddAsync(record);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<AttendeeRecordDto>.SuccessResponse(record.Adapt<AttendeeRecordDto>(), null, 201);
    }

    public async Task<ApiResponse<AttendeeRecordDto>> Update(int id, CreateAttendeeRecordDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<AttendeeRecordDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var record = await _unitOfWork.AttendeeRecords.GetByIdAsync(id);
        if (record is null)
            return ApiResponse<AttendeeRecordDto>.FailureResponse($"AttendeeRecord with id {id} not found.", 404);

        var eventExists = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (eventExists is null)
            return ApiResponse<AttendeeRecordDto>.FailureResponse($"Event with id {request.EventId} not found.", 404);

        var memberExists = await _unitOfWork.Members.GetByIdAsync(request.MemberId);
        if (memberExists is null)
            return ApiResponse<AttendeeRecordDto>.FailureResponse($"Member with id {request.MemberId} not found.", 404);

        request.Adapt(record);
        await _unitOfWork.AttendeeRecords.UpdateAsync(record);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<AttendeeRecordDto>.SuccessResponse(record.Adapt<AttendeeRecordDto>());
    }

    public async Task<ApiResponse<AttendeeRecordDto>> Delete(int id)
    {
        var record = await _unitOfWork.AttendeeRecords.GetByIdAsync(id);
        if (record is null)
            return ApiResponse<AttendeeRecordDto>.FailureResponse($"AttendeeRecord with id {id} not found.", 404);

        await _unitOfWork.AttendeeRecords.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<AttendeeRecordDto>.SuccessResponse(null);
    }
}