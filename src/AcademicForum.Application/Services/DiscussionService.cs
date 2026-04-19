using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;
using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using FluentValidation;
using Mapster;

namespace AcademicForum.Application.Services;

public class DiscussionService : IDiscussionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateDiscussionDto> _validator;

    public DiscussionService(IUnitOfWork unitOfWork, IValidator<CreateDiscussionDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ApiResponse<List<DiscussionDto>>> GetAll()
    {
        var discussions = await _unitOfWork.Discussions.GetAllAsync();
        return ApiResponse<List<DiscussionDto>>.SuccessResponse(discussions.Adapt<List<DiscussionDto>>());
    }

    public async Task<ApiResponse<DiscussionDto>> GetById(int id)
    {
        var discussion = await _unitOfWork.Discussions.GetByIdAsync(id);

        return discussion is null
            ? ApiResponse<DiscussionDto>.FailureResponse($"Discussion with id {id} not found.", 404)
            : ApiResponse<DiscussionDto>.SuccessResponse(discussion.Adapt<DiscussionDto>());
    }

    public async Task<ApiResponse<DiscussionDto>> Create(CreateDiscussionDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<DiscussionDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var movieExists = await _unitOfWork.Movies.GetByIdAsync(request.MovieId);
        if (movieExists is null)
            return ApiResponse<DiscussionDto>.FailureResponse($"Movie with id {request.MovieId} not found.", 404);

        var eventExists = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (eventExists is null)
            return ApiResponse<DiscussionDto>.FailureResponse($"Event with id {request.EventId} not found.", 404);

        var memberExists = await _unitOfWork.Members.GetByIdAsync(request.MemberId);
        if (memberExists is null)
            return ApiResponse<DiscussionDto>.FailureResponse($"Member with id {request.MemberId} not found.", 404);

        var discussion = request.Adapt<Discussion>();
        await _unitOfWork.Discussions.AddAsync(discussion);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<DiscussionDto>.SuccessResponse(discussion.Adapt<DiscussionDto>(), null, 201);
    }

    public async Task<ApiResponse<DiscussionDto>> Update(int id, CreateDiscussionDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<DiscussionDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var discussion = await _unitOfWork.Discussions.GetByIdAsync(id);
        if (discussion is null)
            return ApiResponse<DiscussionDto>.FailureResponse($"Discussion with id {id} not found.", 404);

        var movieExists = await _unitOfWork.Movies.GetByIdAsync(request.MovieId);
        if (movieExists is null)
            return ApiResponse<DiscussionDto>.FailureResponse($"Movie with id {request.MovieId} not found.", 404);

        var eventExists = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (eventExists is null)
            return ApiResponse<DiscussionDto>.FailureResponse($"Event with id {request.EventId} not found.", 404);

        var memberExists = await _unitOfWork.Members.GetByIdAsync(request.MemberId);
        if (memberExists is null)
            return ApiResponse<DiscussionDto>.FailureResponse($"Member with id {request.MemberId} not found.", 404);

        request.Adapt(discussion);
        await _unitOfWork.Discussions.UpdateAsync(discussion);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<DiscussionDto>.SuccessResponse(discussion.Adapt<DiscussionDto>());
    }

    public async Task<ApiResponse<DiscussionDto>> Delete(int id)
    {
        var discussion = await _unitOfWork.Discussions.GetByIdAsync(id);
        if (discussion is null)
            return ApiResponse<DiscussionDto>.FailureResponse($"Discussion with id {id} not found.", 404);

        await _unitOfWork.Discussions.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<DiscussionDto>.SuccessResponse(null);
    }
}