using AcademicForum.Application.DTOs;
using AcademicForum.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademicForum.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _memberService.GetAll();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _memberService.GetById(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberDto memberRequest)
        {
            var response = await _memberService.Create(memberRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateMemberDto memberRequest)
        {
            var response = await _memberService.Update(id, memberRequest);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _memberService.Delete(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
