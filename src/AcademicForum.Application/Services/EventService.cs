using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;
using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using FluentValidation;
using Mapster;

namespace AcademicForum.Application.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateEventDto> _validator;

    public EventService(IUnitOfWork unitOfWork, IValidator<CreateEventDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<ApiResponse<List<EventDto>>> GetAll()
    {
        var events = await _unitOfWork.Events.GetAllAsync();
        return ApiResponse<List<EventDto>>.SuccessResponse(events.Adapt<List<EventDto>>());
    }

    public async Task<ApiResponse<EventDto>> GetById(int id)
    {
        var ev = await _unitOfWork.Events.GetByIdAsync(id);

        return ev is null
            ? ApiResponse<EventDto>.FailureResponse($"Event with id {id} not found.", 404)
            : ApiResponse<EventDto>.SuccessResponse(ev.Adapt<EventDto>());
    }

    public async Task<ApiResponse<EventDto>> Create(CreateEventDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<EventDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var movieExists = await _unitOfWork.Movies.GetByIdAsync(request.MovieId);
        if (movieExists is null)
            return ApiResponse<EventDto>.FailureResponse($"Movie with id {request.MovieId} not found.", 404);

        var venueExists = await _unitOfWork.Venues.GetByIdAsync(request.VenueId);
        if (venueExists is null)
            return ApiResponse<EventDto>.FailureResponse($"Venue with id {request.VenueId} not found.", 404);

        var ev = request.Adapt<Event>();
        await _unitOfWork.Events.AddAsync(ev);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<EventDto>.SuccessResponse(ev.Adapt<EventDto>(), null, 201);
    }

    public async Task<ApiResponse<EventDto>> Update(int id, CreateEventDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<EventDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var ev = await _unitOfWork.Events.GetByIdAsync(id);
        if (ev is null)
            return ApiResponse<EventDto>.FailureResponse($"Event with id {id} not found.", 404);

        var movieExists = await _unitOfWork.Movies.GetByIdAsync(request.MovieId);
        if (movieExists is null)
            return ApiResponse<EventDto>.FailureResponse($"Movie with id {request.MovieId} not found.", 404);

        var venueExists = await _unitOfWork.Venues.GetByIdAsync(request.VenueId);
        if (venueExists is null)
            return ApiResponse<EventDto>.FailureResponse($"Venue with id {request.VenueId} not found.", 404);

        request.Adapt(ev);
        await _unitOfWork.Events.UpdateAsync(ev);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<EventDto>.SuccessResponse(ev.Adapt<EventDto>());
    }

    public async Task<ApiResponse<EventDto>> Delete(int id)
    {
        var ev = await _unitOfWork.Events.GetByIdAsync(id);
        if (ev is null)
            return ApiResponse<EventDto>.FailureResponse($"Event with id {id} not found.", 404);

        await _unitOfWork.Events.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<EventDto>.SuccessResponse(null);
    }
}