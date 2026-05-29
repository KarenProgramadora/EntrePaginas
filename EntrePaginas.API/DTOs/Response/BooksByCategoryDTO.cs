namespace EntrePaginas.API.DTOs.Response;

public class BooksByCategoryDTO
{
    public string CategoryName { get; set; } = string.Empty;
    public int BookCount { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}
