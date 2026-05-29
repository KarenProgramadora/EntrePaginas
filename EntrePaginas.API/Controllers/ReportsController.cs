using AutoMapper;
using EntrePaginas.API.DTOs.Response;
using EntrePaginas.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntrePaginas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _reportsService;
    private readonly IMapper _mapper;

    public ReportsController(IReportsService reportsService, IMapper mapper)
    {
        _reportsService = reportsService;
        _mapper = mapper;
    }

    /// <summary>Resumen general de la biblioteca.</summary>
    [HttpGet("stats")]
    public async Task<ActionResult<LibraryStatsDTO>> GetLibraryStats()
    {
        var stats = await _reportsService.GetLibraryStatsAsync();
        return Ok(_mapper.Map<LibraryStatsDTO>(stats));
    }

    /// <summary>Cantidad de libros y copias agrupados por categoría.</summary>
    [HttpGet("books-by-category")]
    public async Task<ActionResult<IEnumerable<BooksByCategoryDTO>>> GetBooksByCategory()
    {
        var result = await _reportsService.GetBooksByCategoryAsync();
        return Ok(_mapper.Map<IEnumerable<BooksByCategoryDTO>>(result));
    }

    /// <summary>Libros más prestados (top N, por defecto 5).</summary>
    [HttpGet("most-loaned")]
    public async Task<ActionResult<IEnumerable<MostLoanedBookDTO>>> GetMostLoaned([FromQuery] int top = 5)
    {
        var result = await _reportsService.GetMostLoanedAsync(top);
        return Ok(_mapper.Map<IEnumerable<MostLoanedBookDTO>>(result));
    }

    /// <summary>Actividad de préstamos y multas por miembro.</summary>
    [HttpGet("member-activity")]
    public async Task<ActionResult<IEnumerable<MemberActivityDTO>>> GetMemberActivity()
    {
        var result = await _reportsService.GetMemberActivityAsync();
        return Ok(_mapper.Map<IEnumerable<MemberActivityDTO>>(result));
    }

    /// <summary>Préstamos activos próximos a vencer en los próximos N días.</summary>
    [HttpGet("loans-due-soon")]
    public async Task<ActionResult<IEnumerable<LoanDueSoonDTO>>> GetLoansDueSoon([FromQuery] int days = 3)
    {
        var result = await _reportsService.GetLoansDueSoonAsync(days);
        return Ok(_mapper.Map<IEnumerable<LoanDueSoonDTO>>(result));
    }
}
