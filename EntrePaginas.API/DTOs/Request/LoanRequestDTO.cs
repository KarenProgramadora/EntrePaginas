namespace EntrePaginas.API.DTOs.Request;

public class LoanRequestDTO
{
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public int LoanDays { get; set; } = 14;
}
