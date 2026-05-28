using EntrePaginas.Domain.Enums;

namespace EntrePaginas.API.DTOs.Request;

public class BookRequestDTO
{
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public BookCondition Condition { get; set; } = BookCondition.New;
    public string? CoverUrl { get; set; }
    public int CategoryId { get; set; }
    public int PublisherId { get; set; }
}
