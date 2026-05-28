using EntrePaginas.Domain.Enums;

namespace EntrePaginas.Domain.Entities;

public class Loan : AuditBase
{
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Active;

    public Book Book { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public Fine? Fine { get; set; }
}
