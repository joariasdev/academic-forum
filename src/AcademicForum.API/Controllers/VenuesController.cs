using AcademicForum.Application.DTOs;
using AcademicForum.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademicForum.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _venueService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _venueService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVenueDto venueRequest)
        {
            var response = await _venueService.Create(venueRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateVenueDto venueRequest)
        {
            var response = await _venueService.Update(id, venueRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _venueService.Delete(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
