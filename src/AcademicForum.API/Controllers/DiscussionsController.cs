using AcademicForum.Application.DTOs;
using AcademicForum.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademicForum.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscussionsController : ControllerBase
    {
        private readonly IDiscussionService _discussionService;

        public DiscussionsController(IDiscussionService discussionService)
        {
            _discussionService = discussionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _discussionService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _discussionService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDiscussionDto discussionRequest)
        {
            var response = await _discussionService.Create(discussionRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateDiscussionDto discussionRequest)
        {
            var response = await _discussionService.Update(id, discussionRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _discussionService.Delete(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
