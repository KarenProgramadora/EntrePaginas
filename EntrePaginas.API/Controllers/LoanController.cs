using AutoMapper;
using EntrePaginas.API.DTOs.Request;
using EntrePaginas.API.DTOs.Response;
using EntrePaginas.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntrePaginas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanController : ControllerBase
{
    private readonly ILoanService _service;
    private readonly IMapper _mapper;

    public LoanController(ILoanService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoanResponseDTO>>> GetAll()
    {
        var loans = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<LoanResponseDTO>>(loans));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LoanResponseDTO>> GetById(int id)
    {
        var loan = await _service.GetByIdWithDetailsAsync(id);
        if (loan == null) return NotFound();
        return Ok(_mapper.Map<LoanResponseDTO>(loan));
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<LoanResponseDTO>>> GetByMember(int memberId)
    {
        var loans = await _service.GetByMemberIdAsync(memberId);
        return Ok(_mapper.Map<IEnumerable<LoanResponseDTO>>(loans));
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<LoanResponseDTO>>> GetOverdue()
    {
        var loans = await _service.GetOverdueLoansAsync();
        return Ok(_mapper.Map<IEnumerable<LoanResponseDTO>>(loans));
    }

    [HttpPost]
    public async Task<ActionResult<LoanResponseDTO>> Create([FromBody] CreateLoanRequestDTO dto)
    {
        try
        {
            var loan = await _service.CreateLoanAsync(dto.MemberId, dto.BookId, dto.LoanDays);
            return CreatedAtAction(nameof(GetById), new { id = loan.Id },
                _mapper.Map<LoanResponseDTO>(loan));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/return")]
    public async Task<IActionResult> Return(int id)
    {
        try
        {
            await _service.ReturnLoanAsync(id);
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

    [HttpPost("mark-overdue")]
    public async Task<IActionResult> MarkOverdue()
    {
        await _service.MarkOverdueLoansAsync();
        return NoContent();
    }
}
