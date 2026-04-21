using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;
using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using FluentValidation;
using Mapster;

namespace AcademicForum.Application.Services;

public class VenueService : IVenueService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateVenueDto> _validator;

    public VenueService(IUnitOfWork unitOfWork, IValidator<CreateVenueDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator  = validator;
    }

    public async Task<ApiResponse<List<VenueDto>>> GetAll()
    {
        var venues = await _unitOfWork.Venues.GetAllAsync();
        return ApiResponse<List<VenueDto>>.SuccessResponse(venues.Adapt<List<VenueDto>>());
    }

    public async Task<ApiResponse<VenueDetailDto>> GetById(int id)
    {
        var venue = await _unitOfWork.Venues.GetByIdAsync(id);

        return venue is null
            ? ApiResponse<VenueDetailDto>.FailureResponse($"Venue with id {id} not found.", 404)
            : ApiResponse<VenueDetailDto>.SuccessResponse(venue.Adapt<VenueDetailDto>());
    }

    public async Task<ApiResponse<VenueDto>> Create(CreateVenueDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<VenueDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var venue = request.Adapt<Venue>();

        await _unitOfWork.Venues.AddAsync(venue);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<VenueDto>.SuccessResponse(venue.Adapt<VenueDto>(), null, 201);
    }

    public async Task<ApiResponse<VenueDto>> Update(int id, CreateVenueDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<VenueDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var venue = await _unitOfWork.Venues.GetByIdAsync(id);
        if (venue is null)
            return ApiResponse<VenueDto>.FailureResponse($"Venue with id {id} not found.", 404);

        request.Adapt(venue);

        await _unitOfWork.Venues.UpdateAsync(venue);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<VenueDto>.SuccessResponse(venue.Adapt<VenueDto>());
    }

    public async Task<ApiResponse<VenueDto>> Delete(int id)
    {
        var venue = await _unitOfWork.Venues.GetByIdAsync(id);
        if (venue is null)
            return ApiResponse<VenueDto>.FailureResponse($"Venue with id {id} not found.", 404);

        await _unitOfWork.Venues.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<VenueDto>.SuccessResponse(null);
    }
}