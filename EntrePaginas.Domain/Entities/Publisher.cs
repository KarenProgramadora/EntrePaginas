namespace EntrePaginas.Domain.Entities;

public class Publisher : AuditBase
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Website { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
