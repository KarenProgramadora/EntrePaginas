namespace EntrePaginas.API.DTOs.Response;

public class MemberActivityDTO
{
    public int MemberId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int TotalLoans { get; set; }
    public int ActiveLoans { get; set; }
    public int OverdueLoans { get; set; }
    public decimal TotalFines { get; set; }
}
