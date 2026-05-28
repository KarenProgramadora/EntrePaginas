using AutoMapper;
using EntrePaginas.API.DTOs.Request;
using EntrePaginas.API.DTOs.Response;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EntrePaginas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IGenericRepository<Author> _repository;
    private readonly IMapper _mapper;

    public AuthorController(IGenericRepository<Author> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorResponseDTO>>> GetAll()
    {
        var authors = await _repository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<AuthorResponseDTO>>(authors));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorResponseDTO>> GetById(int id)
    {
        var author = await _repository.GetByIdAsync(id);
        if (author == null) return NotFound();
        return Ok(_mapper.Map<AuthorResponseDTO>(author));
    }

    [HttpPost]
    public async Task<ActionResult<AuthorResponseDTO>> Create([FromBody] AuthorRequestDTO dto)
    {
        var author = _mapper.Map<Author>(dto);
        var created = await _repository.CreateAsync(author);
        return CreatedAtAction(nameof(GetById), new { id = created.Id },
            _mapper.Map<AuthorResponseDTO>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AuthorRequestDTO dto)
    {
        if (!await _repository.ExistsAsync(id)) return NotFound();
        var author = _mapper.Map<Author>(dto);
        author.Id = id;
        await _repository.UpdateAsync(author);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _repository.ExistsAsync(id)) return NotFound();
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
