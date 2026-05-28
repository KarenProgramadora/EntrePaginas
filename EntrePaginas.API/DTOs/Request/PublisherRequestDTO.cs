namespace EntrePaginas.API.DTOs.Request;

public class PublisherRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Website { get; set; }
}
