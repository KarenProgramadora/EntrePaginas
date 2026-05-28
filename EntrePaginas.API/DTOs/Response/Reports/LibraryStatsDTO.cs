namespace EntrePaginas.API.DTOs.Response.Reports;

public class LibraryStatsDTO
{
    public int TotalBooks { get; set; }
    public int TotalAvailableCopies { get; set; }
    public int TotalLoanedCopies { get; set; }
    public int TotalAuthors { get; set; }
    public int TotalMembers { get; set; }
    public int ActiveMembers { get; set; }
    public int ActiveLoans { get; set; }
    public int OverdueLoans { get; set; }
    public int UnpaidFines { get; set; }
    public decimal TotalUnpaidFinesAmount { get; set; }
}
