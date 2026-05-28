using EntrePaginas.Domain.Enums;

namespace EntrePaginas.Domain.Entities;

public class Book : AuditBase
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

    public Category Category { get; set; } = null!;
    public Publisher Publisher { get; set; } = null!;
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
