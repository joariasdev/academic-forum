using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;
using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using FluentValidation;
using Mapster;

namespace AcademicForum.Application.Services;

public class ResponseService : IResponseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateResponseDto> _validator;

    public ResponseService(IUnitOfWork unitOfWork, IValidator<CreateResponseDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ApiResponse<List<ResponseDto>>> GetAll()
    {
        var responses = await _unitOfWork.Responses.GetAllAsync();
        return ApiResponse<List<ResponseDto>>.SuccessResponse(responses.Adapt<List<ResponseDto>>());
    }

    public async Task<ApiResponse<ResponseDto>> GetById(int id)
    {
        var response = await _unitOfWork.Responses.GetByIdAsync(id);

        return response is null
            ? ApiResponse<ResponseDto>.FailureResponse($"Response with id {id} not found.", 404)
            : ApiResponse<ResponseDto>.SuccessResponse(response.Adapt<ResponseDto>());
    }

    public async Task<ApiResponse<ResponseDto>> Create(CreateResponseDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<ResponseDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var discussionExists = await _unitOfWork.Discussions.GetByIdAsync(request.DiscussionId);
        if (discussionExists is null)
            return ApiResponse<ResponseDto>.FailureResponse($"Discussion with id {request.DiscussionId} not found.", 404);

        var memberExists = await _unitOfWork.Members.GetByIdAsync(request.MemberId);
        if (memberExists is null)
            return ApiResponse<ResponseDto>.FailureResponse($"Member with id {request.MemberId} not found.", 404);

        var response = request.Adapt<Response>();
        await _unitOfWork.Responses.AddAsync(response);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<ResponseDto>.SuccessResponse(response.Adapt<ResponseDto>(), null, 201);
    }

    public async Task<ApiResponse<ResponseDto>> Update(int id, CreateResponseDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<ResponseDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var response = await _unitOfWork.Responses.GetByIdAsync(id);
        if (response is null)
            return ApiResponse<ResponseDto>.FailureResponse($"Response with id {id} not found.", 404);

        var discussionExists = await _unitOfWork.Discussions.GetByIdAsync(request.DiscussionId);
        if (discussionExists is null)
            return ApiResponse<ResponseDto>.FailureResponse($"Discussion with id {request.DiscussionId} not found.", 404);

        var memberExists = await _unitOfWork.Members.GetByIdAsync(request.MemberId);
        if (memberExists is null)
            return ApiResponse<ResponseDto>.FailureResponse($"Member with id {request.MemberId} not found.", 404);

        request.Adapt(response);
        await _unitOfWork.Responses.UpdateAsync(response);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<ResponseDto>.SuccessResponse(response.Adapt<ResponseDto>());
    }

    public async Task<ApiResponse<ResponseDto>> Delete(int id)
    {
        var response = await _unitOfWork.Responses.GetByIdAsync(id);
        if (response is null)
            return ApiResponse<ResponseDto>.FailureResponse($"Response with id {id} not found.", 404);

        await _unitOfWork.Responses.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<ResponseDto>.SuccessResponse(null);
    }
}