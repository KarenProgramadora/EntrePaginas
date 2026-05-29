using EntrePaginas.Domain.Interfaces.Repositories;
using EntrePaginas.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EntrePaginas.Domain.Services;

public class ReportsService : IReportsService
{
    private readonly IReportsRepository _reportsRepository;
    private readonly ILogger<ReportsService> _logger;

    public ReportsService(IReportsRepository reportsRepository, ILogger<ReportsService> logger)
    {
        _reportsRepository = reportsRepository;
        _logger = logger;
    }

    public async Task<LibraryStatsRaw> GetLibraryStatsAsync()
    {
        _logger.LogInformation("Retrieving library stats");
        return await _reportsRepository.GetLibraryStatsAsync();
    }

    public async Task<IEnumerable<BooksByCategoryRaw>> GetBooksByCategoryAsync()
    {
        _logger.LogInformation("Retrieving books grouped by category");
        return await _reportsRepository.GetBooksByCategoryAsync();
    }

    public async Task<IEnumerable<MostLoanedBookRaw>> GetMostLoanedAsync(int top)
    {
        _logger.LogInformation("Retrieving top {Top} most loaned books", top);
        return await _reportsRepository.GetMostLoanedAsync(top);
    }

    public async Task<IEnumerable<MemberActivityRaw>> GetMemberActivityAsync()
    {
        _logger.LogInformation("Retrieving member activity");
        return await _reportsRepository.GetMemberActivityAsync();
    }

    public async Task<IEnumerable<LoanDueSoonRaw>> GetLoansDueSoonAsync(int days)
    {
        _logger.LogInformation("Retrieving loans due in the next {Days} days", days);
        return await _reportsRepository.GetLoansDueSoonAsync(days);
    }
}
