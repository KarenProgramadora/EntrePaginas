using AutoMapper;
using EntrePaginas.API.DTOs.Request;
using EntrePaginas.API.DTOs.Response;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntrePaginas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FineController : ControllerBase
{
    private readonly IFineService _service;
    private readonly IMapper _mapper;

    public FineController(IFineService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FineResponseDTO>>> GetAll()
    {
        var fines = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<FineResponseDTO>>(fines));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FineResponseDTO>> GetById(int id)
    {
        var fine = await _service.GetByIdAsync(id);
        if (fine == null) return NotFound();
        return Ok(_mapper.Map<FineResponseDTO>(fine));
    }

    [HttpGet("unpaid")]
    public async Task<ActionResult<IEnumerable<FineResponseDTO>>> GetUnpaid()
    {
        var fines = await _service.GetUnpaidFinesAsync();
        return Ok(_mapper.Map<IEnumerable<FineResponseDTO>>(fines));
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<FineResponseDTO>>> GetByMember(int memberId)
    {
        var fines = await _service.GetByMemberIdAsync(memberId);
        return Ok(_mapper.Map<IEnumerable<FineResponseDTO>>(fines));
    }

    [HttpPost]
    public async Task<ActionResult<FineResponseDTO>> Create([FromBody] FineRequestDTO dto)
    {
        try
        {
            var fine = _mapper.Map<Fine>(dto);
            var created = await _service.CreateAsync(fine);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                _mapper.Map<FineResponseDTO>(created));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/pay")]
    public async Task<IActionResult> Pay(int id)
    {
        try
        {
            await _service.PayFineAsync(id);
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
