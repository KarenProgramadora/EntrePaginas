using EntrePaginas.DataAccess.Context;
using EntrePaginas.Domain.Enums;
using EntrePaginas.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EntrePaginas.DataAccess.Repositories;

public class ReportsRepository : IReportsRepository
{
    private readonly EntrePaginasDbContext _context;

    public ReportsRepository(EntrePaginasDbContext context)
    {
        _context = context;
    }

    public async Task<LibraryStatsRaw> GetLibraryStatsAsync()
    {
        return new LibraryStatsRaw(
            TotalBooks: await _context.Books.CountAsync(),
            TotalAvailableCopies: await _context.Books.SumAsync(b => b.AvailableCopies),
            TotalLoanedCopies: await _context.Books.SumAsync(b => b.TotalCopies - b.AvailableCopies),
            TotalAuthors: await _context.Authors.CountAsync(),
            TotalMembers: await _context.Members.CountAsync(),
            ActiveMembers: await _context.Members.CountAsync(m => m.Status == MemberStatus.Active),
            ActiveLoans: await _context.Loans.CountAsync(l => l.Status == LoanStatus.Active),
            OverdueLoans: await _context.Loans.CountAsync(l => l.Status == LoanStatus.Overdue ||
                                      (l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow)),
            UnpaidFines: await _context.Fines.CountAsync(f => !f.IsPaid),
            TotalUnpaidFinesAmount: await _context.Fines
                                        .Where(f => !f.IsPaid)
                                        .SumAsync(f => (decimal?)f.Amount) ?? 0
        );
    }

    public async Task<IEnumerable<BooksByCategoryRaw>> GetBooksByCategoryAsync()
        => await _context.Books
            .Include(b => b.Category)
            .GroupBy(b => b.Category.Name)
            .Select(g => new BooksByCategoryRaw(
                g.Key,
                g.Count(),
                g.Sum(b => b.TotalCopies),
                g.Sum(b => b.AvailableCopies)))
            .OrderByDescending(x => x.BookCount)
            .ToListAsync();

    public async Task<IEnumerable<MostLoanedBookRaw>> GetMostLoanedAsync(int top)
        => await _context.Loans
            .Include(l => l.Book)
                .ThenInclude(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
            .GroupBy(l => new { l.BookId, l.Book.Title })
            .Select(g => new MostLoanedBookRaw(
                g.Key.BookId,
                g.Key.Title,
                g.First().Book.BookAuthors
                    .Select(ba => ba.Author.FirstName + " " + ba.Author.LastName)
                    .FirstOrDefault() ?? string.Empty,
                g.Count()))
            .OrderByDescending(x => x.LoanCount)
            .Take(top)
            .ToListAsync();

    public async Task<IEnumerable<MemberActivityRaw>> GetMemberActivityAsync()
        => await _context.Members
            .Select(m => new MemberActivityRaw(
                m.Id,
                m.FirstName + " " + m.LastName,
                m.Loans.Count,
                m.Loans.Count(l => l.Status == LoanStatus.Active),
                m.Loans.Count(l => l.Status == LoanStatus.Overdue ||
                                  (l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow)),
                m.Loans
                    .Where(l => l.Fine != null && !l.Fine.IsPaid)
                    .Sum(l => (decimal?)l.Fine!.Amount) ?? 0))
            .OrderByDescending(x => x.TotalLoans)
            .ToListAsync();

    public async Task<IEnumerable<LoanDueSoonRaw>> GetLoansDueSoonAsync(int days)
    {
        var limit = DateTime.UtcNow.AddDays(days);
        var now = DateTime.UtcNow;

        return await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.Status == LoanStatus.Active && l.DueDate <= limit)
            .OrderBy(l => l.DueDate)
            .Select(l => new LoanDueSoonRaw(
                l.Id,
                l.Book.Title,
                l.Member.FirstName + " " + l.Member.LastName,
                l.Member.Email,
                l.DueDate,
                (int)(l.DueDate - now).TotalDays))
            .ToListAsync();
    }
}
