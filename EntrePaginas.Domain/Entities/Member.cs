using EntrePaginas.Domain.Enums;

namespace EntrePaginas.Domain.Entities;

public class Member : AuditBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime MembershipDate { get; set; }
    public MemberStatus Status { get; set; } = MemberStatus.Active;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
