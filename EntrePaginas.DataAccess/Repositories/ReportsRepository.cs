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
    {
        // Materializamos antes de proyectar a record (EF no traduce GroupBy → ctor de record)
        var books = await _context.Books
            .Include(b => b.Category)
            .AsNoTracking()
            .ToListAsync();

        return books
            .GroupBy(b => b.Category != null ? b.Category.Name : "Sin categoría")
            .Select(g => new BooksByCategoryRaw(
                g.Key,
                g.Count(),
                g.Sum(b => b.TotalCopies),
                g.Sum(b => b.AvailableCopies)))
            .OrderByDescending(x => x.BookCount)
            .ToList();
    }

    public async Task<IEnumerable<MostLoanedBookRaw>> GetMostLoanedAsync(int top)
    {
        // Paso 1: contar préstamos por libro (traducible a SQL puro)
        var loanCounts = await _context.Loans
            .GroupBy(l => l.BookId)
            .Select(g => new { BookId = g.Key, LoanCount = g.Count() })
            .OrderByDescending(x => x.LoanCount)
            .Take(top)
            .ToListAsync();

        if (loanCounts.Count == 0)
            return new List<MostLoanedBookRaw>();

        var bookIds = loanCounts.Select(x => x.BookId).ToList();

        // Paso 2: cargar detalles solo de los libros del top
        var books = await _context.Books
            .Where(b => bookIds.Contains(b.Id))
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .AsNoTracking()
            .ToListAsync();

        // Paso 3: combinar en memoria
        return loanCounts
            .Select(lc =>
            {
                var book = books.First(b => b.Id == lc.BookId);
                var author = book.BookAuthors
                    .Select(ba => ba.Author.FirstName + " " + ba.Author.LastName)
                    .FirstOrDefault() ?? string.Empty;
                return new MostLoanedBookRaw(book.Id, book.Title, author, lc.LoanCount);
            })
            .ToList();
    }

    public async Task<IEnumerable<MemberActivityRaw>> GetMemberActivityAsync()
    {
        // Materializamos los miembros con sus préstamos y multas antes de proyectar a record
        var members = await _context.Members
            .Include(m => m.Loans)
                .ThenInclude(l => l.Fine)
            .AsNoTracking()
            .ToListAsync();

        var now = DateTime.UtcNow;

        return members
            .Select(m => new MemberActivityRaw(
                m.Id,
                m.FirstName + " " + m.LastName,
                m.Loans.Count,
                m.Loans.Count(l => l.Status == LoanStatus.Active),
                m.Loans.Count(l => l.Status == LoanStatus.Overdue ||
                                  (l.Status == LoanStatus.Active && l.DueDate < now)),
                m.Loans
                    .Where(l => l.Fine != null && !l.Fine.IsPaid)
                    .Sum(l => l.Fine!.Amount)))
            .OrderByDescending(x => x.TotalLoans)
            .ToList();
    }

    public async Task<IEnumerable<LoanDueSoonRaw>> GetLoansDueSoonAsync(int days)
    {
        var now = DateTime.UtcNow;
        var limit = now.AddDays(days);

        var loans = await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.Status == LoanStatus.Active && l.DueDate <= limit)
            .OrderBy(l => l.DueDate)
            .AsNoTracking()
            .ToListAsync();

        return loans
            .Select(l => new LoanDueSoonRaw(
                l.Id,
                l.Book.Title,
                l.Member.FirstName + " " + l.Member.LastName,
                l.Member.Email,
                l.DueDate,
                (int)(l.DueDate - now).TotalDays))
            .ToList();
    }
}
