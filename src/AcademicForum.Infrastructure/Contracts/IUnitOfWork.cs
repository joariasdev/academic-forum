using AcademicForum.Infrastructure.Repositories;

namespace AcademicForum.Infrastructure.Contracts;

public interface IUnitOfWork
    {
        MovieRepository Movies { get; }
        MemberRepository Members { get; }
        VenueRepository Venues { get; }
        DiscussionRepository Discussions { get; }
        ResponseRepository Responses { get; }
        AttendeeRecordRepository AttendeeRecords { get; }
        EventRepository Events { get; }
        Task CompleteAsync();
    }
