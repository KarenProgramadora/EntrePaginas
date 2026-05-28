namespace EntrePaginas.Domain.Entities;

public class Fine : AuditBase
{
    public int LoanId { get; set; }
    public decimal Amount { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public bool IsPaid { get; set; } = false;
    public string? Notes { get; set; }

    public Loan Loan { get; set; } = null!;
}
