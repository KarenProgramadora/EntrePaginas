namespace EntrePaginas.API.DTOs.Response;

public class FineResponseDTO
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public decimal Amount { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public bool IsPaid { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
