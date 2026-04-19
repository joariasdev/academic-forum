using AcademicForum.Application.Services;
using AcademicForum.Infrastructure.Contracts;
using AcademicForum.Infrastructure.Data;
using AcademicForum.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using AcademicForum.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddValidatorsFromAssemblyContaining<CreateMovieDtoValidator>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<VenueRepository>();
builder.Services.AddScoped<DiscussionRepository>();
builder.Services.AddScoped<ResponseRepository>();
builder.Services.AddScoped<AttendeeRecordRepository>();
builder.Services.AddScoped<EventRepository>();

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<IResponseService, ResponseService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddScoped<IAttendeeRecordService, AttendeeRecordService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("ReactDev");

app.UseAuthorization();

app.MapControllers();

app.Run();
