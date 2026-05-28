namespace EntrePaginas.API.DTOs.Response.Reports;

public class MostLoanedBookDTO
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public int LoanCount { get; set; }
}
