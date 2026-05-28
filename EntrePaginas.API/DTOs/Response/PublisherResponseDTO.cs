namespace EntrePaginas.API.DTOs.Response;

public class PublisherResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Website { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
