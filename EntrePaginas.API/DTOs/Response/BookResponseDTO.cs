using EntrePaginas.Domain.Enums;

namespace EntrePaginas.API.DTOs.Response;

public class BookResponseDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public BookCondition Condition { get; set; }
    public string ConditionName { get; set; } = string.Empty;
    public string? CoverUrl { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int PublisherId { get; set; }
    public string PublisherName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
