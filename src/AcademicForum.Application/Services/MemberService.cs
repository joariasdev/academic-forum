using AcademicForum.Application.DTOs;
using AcademicForum.Application.Responses;
using AcademicForum.Domain.Entities;
using AcademicForum.Infrastructure.Contracts;
using FluentValidation;
using Mapster;

namespace AcademicForum.Application.Services;

public class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateMemberDto> _validator;

    public MemberService(IUnitOfWork unitOfWork, IValidator<CreateMemberDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator  = validator;
    }

    public async Task<ApiResponse<List<MemberDto>>> GetAll()
    {
        var members = await _unitOfWork.Members.GetAllAsync();
        return ApiResponse<List<MemberDto>>.SuccessResponse(members.Adapt<List<MemberDto>>());
    }

    public async Task<ApiResponse<MemberDto>> GetById(int id)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);

        return member is null
            ? ApiResponse<MemberDto>.FailureResponse($"Member with id {id} not found.", 404)
            : ApiResponse<MemberDto>.SuccessResponse(member.Adapt<MemberDto>());
    }

    public async Task<ApiResponse<MemberDto>> Create(CreateMemberDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<MemberDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var member = request.Adapt<Member>();

        await _unitOfWork.Members.AddAsync(member);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<MemberDto>.SuccessResponse(member.Adapt<MemberDto>(), null, 201);
    }

    public async Task<ApiResponse<MemberDto>> Update(int id, CreateMemberDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return ApiResponse<MemberDto>.FailureResponse(validation.Errors.First().ErrorMessage);

        var member = await _unitOfWork.Members.GetByIdAsync(id);
        if (member is null)
            return ApiResponse<MemberDto>.FailureResponse($"Member with id {id} not found.", 404);

        request.Adapt(member);

        await _unitOfWork.Members.UpdateAsync(member);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<MemberDto>.SuccessResponse(member.Adapt<MemberDto>());
    }

    public async Task<ApiResponse<MemberDto>> Delete(int id)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(id);
        if (member is null)
            return ApiResponse<MemberDto>.FailureResponse($"Member with id {id} not found.", 404);

        await _unitOfWork.Members.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();

        return ApiResponse<MemberDto>.SuccessResponse(null);
    }
}