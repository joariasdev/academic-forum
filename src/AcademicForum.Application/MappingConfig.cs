using AcademicForum.Application.DTOs;
using AcademicForum.Domain.Entities;
using Mapster;

namespace AcademicForum.Application;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Discussion, DiscussionDto>.NewConfig()
            .Map(dest => dest.MemberName, src => src.Member.Name);

        TypeAdapterConfig<Discussion, DiscussionDetailDto>.NewConfig()
            .Map(dest => dest.MemberName, src => src.Member.Name);

        TypeAdapterConfig<Response, ResponseDto>.NewConfig()
            .Map(dest => dest.MemberName, src => src.Member.Name);

        TypeAdapterConfig<Event, EventDetailDto>.NewConfig()
            .Map(dest => dest.MovieTitle, src => src.Movie.Title)
            .Map(dest => dest.VenueName, src => src.Venue.Name);

        TypeAdapterConfig<AttendeeRecord, AttendeeRecordDto>.NewConfig()
            .Map(dest => dest.MemberName, src => src.Member.Name);

        TypeAdapterConfig<Discussion, DiscussionDto>.NewConfig()
            .Map(dest => dest.MemberName, src => src.Member.Name)
            .Map(dest => dest.ResponseCount, src => src.Responses.Count);

        TypeAdapterConfig<Discussion, DiscussionDetailDto>.NewConfig()
        .Map(dest => dest.MemberName, src => src.Member.Name)
        .Map(dest => dest.ResponseCount, src => src.Responses.Count);
    }
}