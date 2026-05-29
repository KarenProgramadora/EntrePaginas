namespace EntrePaginas.Domain.Interfaces.Repositories;

public interface IReportsRepository
{
    // Cada método encapsula una consulta agregada para no exponer el DbContext
    Task<LibraryStatsRaw> GetLibraryStatsAsync();
    Task<IEnumerable<BooksByCategoryRaw>> GetBooksByCategoryAsync();
    Task<IEnumerable<MostLoanedBookRaw>> GetMostLoanedAsync(int top);
    Task<IEnumerable<MemberActivityRaw>> GetMemberActivityAsync();
    Task<IEnumerable<LoanDueSoonRaw>> GetLoansDueSoonAsync(int days);
}

// ── Proyecciones internas (DTO-like) usadas por la capa de datos ──
// Se mantienen en Domain para no acoplar API ↔ DataAccess

public record LibraryStatsRaw(
    int TotalBooks,
    int TotalAvailableCopies,
    int TotalLoanedCopies,
    int TotalAuthors,
    int TotalMembers,
    int ActiveMembers,
    int ActiveLoans,
    int OverdueLoans,
    int UnpaidFines,
    decimal TotalUnpaidFinesAmount);

public record BooksByCategoryRaw(
    string CategoryName,
    int BookCount,
    int TotalCopies,
    int AvailableCopies);

public record MostLoanedBookRaw(
    int BookId,
    string Title,
    string AuthorName,
    int LoanCount);

public record MemberActivityRaw(
    int MemberId,
    string FullName,
    int TotalLoans,
    int ActiveLoans,
    int OverdueLoans,
    decimal TotalFines);

public record LoanDueSoonRaw(
    int LoanId,
    string BookTitle,
    string MemberName,
    string MemberEmail,
    DateTime DueDate,
    int DaysLeft);
