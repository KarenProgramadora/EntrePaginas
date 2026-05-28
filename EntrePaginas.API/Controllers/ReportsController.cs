using EntrePaginas.API.DTOs.Response.Reports;
using EntrePaginas.DataAccess.Context;
using EntrePaginas.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntrePaginas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly EntrePaginasDbContext _context;

    public ReportsController(EntrePaginasDbContext context)
    {
        _context = context;
    }

    /// <summary>Resumen general de la biblioteca.</summary>
    [HttpGet("stats")]
    public async Task<ActionResult<LibraryStatsDTO>> GetLibraryStats()
    {
        var stats = new LibraryStatsDTO
        {
            TotalBooks          = await _context.Books.CountAsync(),
            TotalAvailableCopies = await _context.Books.SumAsync(b => b.AvailableCopies),
            TotalLoanedCopies   = await _context.Books.SumAsync(b => b.TotalCopies - b.AvailableCopies),
            TotalAuthors        = await _context.Authors.CountAsync(),
            TotalMembers        = await _context.Members.CountAsync(),
            ActiveMembers       = await _context.Members.CountAsync(m => m.Status == MemberStatus.Active),
            ActiveLoans         = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Active),
            OverdueLoans        = await _context.Loans.CountAsync(l => l.Status == LoanStatus.Overdue ||
                                      (l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow)),
            UnpaidFines         = await _context.Fines.CountAsync(f => !f.IsPaid),
            TotalUnpaidFinesAmount = await _context.Fines
                                        .Where(f => !f.IsPaid)
                                        .SumAsync(f => (decimal?)f.Amount) ?? 0
        };

        return Ok(stats);
    }

    /// <summary>Cantidad de libros y copias agrupados por categoría.</summary>
    [HttpGet("books-by-category")]
    public async Task<ActionResult<IEnumerable<BooksByCategoryDTO>>> GetBooksByCategory()
    {
        var result = await _context.Books
            .Include(b => b.Category)
            .GroupBy(b => b.Category.Name)
            .Select(g => new BooksByCategoryDTO
            {
                CategoryName    = g.Key,
                BookCount       = g.Count(),
                TotalCopies     = g.Sum(b => b.TotalCopies),
                AvailableCopies = g.Sum(b => b.AvailableCopies)
            })
            .OrderByDescending(x => x.BookCount)
            .ToListAsync();

        return Ok(result);
    }

    /// <summary>Libros más prestados (top N, por defecto 5).</summary>
    [HttpGet("most-loaned")]
    public async Task<ActionResult<IEnumerable<MostLoanedBookDTO>>> GetMostLoaned([FromQuery] int top = 5)
    {
        var result = await _context.Loans
            .Include(l => l.Book)
                .ThenInclude(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author)
            .GroupBy(l => new { l.BookId, l.Book.Title })
            .Select(g => new MostLoanedBookDTO
            {
                BookId    = g.Key.BookId,
                Title     = g.Key.Title,
                LoanCount = g.Count(),
                AuthorName = g.First().Book.BookAuthors
                              .Select(ba => ba.Author.FirstName + " " + ba.Author.LastName)
                              .FirstOrDefault() ?? string.Empty
            })
            .OrderByDescending(x => x.LoanCount)
            .Take(top)
            .ToListAsync();

        return Ok(result);
    }

    /// <summary>Actividad de préstamos y multas por miembro.</summary>
    [HttpGet("member-activity")]
    public async Task<ActionResult<IEnumerable<MemberActivityDTO>>> GetMemberActivity()
    {
        var result = await _context.Members
            .Select(m => new MemberActivityDTO
            {
                MemberId    = m.Id,
                FullName    = m.FirstName + " " + m.LastName,
                TotalLoans  = m.Loans.Count,
                ActiveLoans = m.Loans.Count(l => l.Status == LoanStatus.Active),
                OverdueLoans = m.Loans.Count(l => l.Status == LoanStatus.Overdue ||
                                  (l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow)),
                TotalFines  = m.Loans
                               .Where(l => l.Fine != null && !l.Fine.IsPaid)
                               .Sum(l => (decimal?)l.Fine!.Amount) ?? 0
            })
            .OrderByDescending(x => x.TotalLoans)
            .ToListAsync();

        return Ok(result);
    }

    /// <summary>Préstamos activos próximos a vencer en los próximos N días.</summary>
    [HttpGet("loans-due-soon")]
    public async Task<ActionResult> GetLoansDueSoon([FromQuery] int days = 3)
    {
        var limit = DateTime.UtcNow.AddDays(days);

        var result = await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.Status == LoanStatus.Active && l.DueDate <= limit)
            .OrderBy(l => l.DueDate)
            .Select(l => new
            {
                LoanId       = l.Id,
                BookTitle    = l.Book.Title,
                MemberName   = l.Member.FirstName + " " + l.Member.LastName,
                MemberEmail  = l.Member.Email,
                DueDate      = l.DueDate,
                DaysLeft     = (int)(l.DueDate - DateTime.UtcNow).TotalDays
            })
            .ToListAsync();

        return Ok(result);
    }
}
