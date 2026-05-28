using AutoMapper;
using EntrePaginas.API.DTOs.Request;
using EntrePaginas.API.DTOs.Response;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EntrePaginas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PublisherController : ControllerBase
{
    private readonly IGenericRepository<Publisher> _repository;
    private readonly IMapper _mapper;

    public PublisherController(IGenericRepository<Publisher> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PublisherResponseDTO>>> GetAll()
    {
        var publishers = await _repository.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<PublisherResponseDTO>>(publishers));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PublisherResponseDTO>> GetById(int id)
    {
        var publisher = await _repository.GetByIdAsync(id);
        if (publisher == null) return NotFound();
        return Ok(_mapper.Map<PublisherResponseDTO>(publisher));
    }

    [HttpPost]
    public async Task<ActionResult<PublisherResponseDTO>> Create([FromBody] PublisherRequestDTO dto)
    {
        var publisher = _mapper.Map<Publisher>(dto);
        var created = await _repository.CreateAsync(publisher);
        return CreatedAtAction(nameof(GetById), new { id = created.Id },
            _mapper.Map<PublisherResponseDTO>(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PublisherRequestDTO dto)
    {
        if (!await _repository.ExistsAsync(id)) return NotFound();
        var publisher = _mapper.Map<Publisher>(dto);
        publisher.Id = id;
        await _repository.UpdateAsync(publisher);
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
