using AcademicForum.Application.DTOs;
using AcademicForum.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademicForum.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendeeRecordsController : ControllerBase
    {
        private readonly IAttendeeRecordService _attendeeRecordService;

        public AttendeeRecordsController(IAttendeeRecordService attendeeRecordService)
        {
            _attendeeRecordService = attendeeRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _attendeeRecordService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _attendeeRecordService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAttendeeRecordDto attendeeRecordRequest)
        {
            var response = await _attendeeRecordService.Create(attendeeRecordRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateAttendeeRecordDto attendeeRecordRequest)
        {
            var response = await _attendeeRecordService.Update(id, attendeeRecordRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _attendeeRecordService.Delete(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
