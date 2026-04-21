using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;
using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using FluentValidation;
using Mapster;

namespace AcademicForum.Application.Services;

public class MovieService : IMovieService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateMovieDto> _validator;

    public MovieService(IUnitOfWork unitOfWork, IValidator<CreateMovieDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator  = validator;
    }

    public async Task<ApiResponse<List<MovieDto>>> GetAll()
    {
        var movies = await _unitOfWork.Movies.GetAllAsync();
        return ApiResponse<List<MovieDto>>.SuccessResponse(movies.Adapt<List<MovieDto>>());
    }

    public async Task<ApiResponse<MovieDetailDto>> GetById(int id)
    {
        var movie = await _unitOfWork.Movies.GetByIdAsync(id);

        return movie is null
            ? ApiResponse<MovieDetailDto>.FailureResponse($"Movie with id {id} not found.", 404)
            : ApiResponse<MovieDetailDto>.SuccessResponse(movie.Adapt<MovieDetailDto>());
    }

    public async Task<ApiResponse<MovieDto>> Create(CreateMovieDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<MovieDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var movie = request.Adapt<Movie>();

        await _unitOfWork.Movies.AddAsync(movie);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<MovieDto>.SuccessResponse(movie.Adapt<MovieDto>(), null, 201);
    }

    public async Task<ApiResponse<MovieDto>> Update(int id, CreateMovieDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<MovieDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var movie = await _unitOfWork.Movies.GetByIdAsync(id);
        if (movie is null)
            return ApiResponse<MovieDto>.FailureResponse($"Movie with id {id} not found.", 404);

        request.Adapt(movie); 

        await _unitOfWork.Movies.UpdateAsync(movie);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<MovieDto>.SuccessResponse(movie.Adapt<MovieDto>());
    }

    public async Task<ApiResponse<MovieDto>> Delete(int id)
    {
        var movie = await _unitOfWork.Movies.GetByIdAsync(id);
        if (movie is null)
            return ApiResponse<MovieDto>.FailureResponse($"Movie with id {id} not found.", 404);

        await _unitOfWork.Movies.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<MovieDto>.SuccessResponse(null);
    }
}