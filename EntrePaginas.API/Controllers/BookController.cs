using AutoMapper;
using EntrePaginas.API.DTOs.Request;
using EntrePaginas.API.DTOs.Response;
using EntrePaginas.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EntrePaginas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _service;
    private readonly IMapper _mapper;

    public BookController(IBookService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetAll()
    {
        var books = await _service.GetAllWithDetailsAsync();
        return Ok(_mapper.Map<IEnumerable<BookResponseDTO>>(books));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookResponseDTO>> GetById(int id)
    {
        var book = await _service.GetByIdWithDetailsAsync(id);
        if (book == null) return NotFound();
        return Ok(_mapper.Map<BookResponseDTO>(book));
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetAvailable()
    {
        var books = await _service.GetAvailableBooksAsync();
        return Ok(_mapper.Map<IEnumerable<BookResponseDTO>>(books));
    }

    [HttpPost]
    public async Task<ActionResult<BookResponseDTO>> Create([FromBody] BookRequestDTO dto)
    {
        try
        {
            var book = _mapper.Map<EntrePaginas.Domain.Entities.Book>(dto);
            var created = await _service.CreateAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                _mapper.Map<BookResponseDTO>(created));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BookRequestDTO dto)
    {
        try
        {
            var book = _mapper.Map<EntrePaginas.Domain.Entities.Book>(dto);
            await _service.UpdateAsync(id, book);
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
