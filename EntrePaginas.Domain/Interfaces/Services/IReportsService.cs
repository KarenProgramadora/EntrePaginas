using EntrePaginas.Domain.Interfaces.Repositories;

namespace EntrePaginas.Domain.Interfaces.Services;

public interface IReportsService
{
    Task<LibraryStatsRaw> GetLibraryStatsAsync();
    Task<IEnumerable<BooksByCategoryRaw>> GetBooksByCategoryAsync();
    Task<IEnumerable<MostLoanedBookRaw>> GetMostLoanedAsync(int top);
    Task<IEnumerable<MemberActivityRaw>> GetMemberActivityAsync();
    Task<IEnumerable<LoanDueSoonRaw>> GetLoansDueSoonAsync(int days);
}
