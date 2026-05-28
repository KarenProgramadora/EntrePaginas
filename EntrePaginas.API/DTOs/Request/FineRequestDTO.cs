namespace EntrePaginas.API.DTOs.Request;

public class FineRequestDTO
{
    public int LoanId { get; set; }
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
}
