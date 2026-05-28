using AutoMapper;
using EntrePaginas.API.DTOs.Request;
using EntrePaginas.API.DTOs.Response;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EntrePaginas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IGenericRepository<Category> _repository;
    private readonly IMapper _mapper;

    public CategoryController(IGenericRepository<Category> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetAll()
    {
        var categories = await _repository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<CategoryResponseDTO>>(categories));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponseDTO>> GetById(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(_mapper.Map<CategoryResponseDTO>(category));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryResponseDTO>> Create([FromBody] CategoryRequestDTO dto)
    {
        var category = _mapper.Map<Category>(dto);
        var created = await _repository.CreateAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = created.Id },
            _mapper.Map<CategoryResponseDTO>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryRequestDTO dto)
    {
        if (!await _repository.ExistsAsync(id)) return NotFound();
        var category = _mapper.Map<Category>(dto);
        category.Id = id;
        await _repository.UpdateAsync(category);
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
