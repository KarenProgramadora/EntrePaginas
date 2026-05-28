using AutoMapper;
using EntrePaginas.API.DTOs.Request;
using EntrePaginas.API.DTOs.Response;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntrePaginas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly IMemberService _service;
    private readonly IMapper _mapper;

    public MemberController(IMemberService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponseDTO>>> GetAll()
    {
        var members = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<MemberResponseDTO>>(members));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberResponseDTO>> GetById(int id)
    {
        var member = await _service.GetByIdWithLoansAsync(id);
        if (member == null) return NotFound();
        return Ok(_mapper.Map<MemberResponseDTO>(member));
    }

    [HttpPost]
    public async Task<ActionResult<MemberResponseDTO>> Create([FromBody] MemberRequestDTO dto)
    {
        try
        {
            var member = _mapper.Map<Member>(dto);
            var created = await _service.CreateAsync(member);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                _mapper.Map<MemberResponseDTO>(created));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MemberRequestDTO dto)
    {
        try
        {
            var member = _mapper.Map<Member>(dto);
            await _service.UpdateAsync(id, member);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
