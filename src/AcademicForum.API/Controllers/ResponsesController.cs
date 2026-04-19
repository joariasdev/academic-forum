using AcademicForum.Application.DTOs;
using AcademicForum.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademicForum.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsesController : ControllerBase
    {
        private readonly IResponseService _responseService;

        public ResponsesController(IResponseService responseService)
        {
            _responseService = responseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _responseService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _responseService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateResponseDto responseRequest)
        {
            var response = await _responseService.Create(responseRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateResponseDto responseRequest)
        {
            var response = await _responseService.Update(id, responseRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _responseService.Delete(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
