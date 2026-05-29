namespace EntrePaginas.API.DTOs.Response;

public class LoanDueSoonDTO
{
    public int LoanId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string MemberName { get; set; } = string.Empty;
    public string MemberEmail { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int DaysLeft { get; set; }
}
