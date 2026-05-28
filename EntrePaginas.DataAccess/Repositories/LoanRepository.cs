using EntrePaginas.DataAccess.Context;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Enums;
using EntrePaginas.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EntrePaginas.DataAccess.Repositories;

public class LoanRepository : GenericRepository<Loan>, ILoanRepository
{
    public LoanRepository(EntrePaginasDbContext context) : base(context) { }

    public override async Task<IEnumerable<Loan>> GetAllAsync()
        => await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Include(l => l.Fine)
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync();

    public async Task<Loan?> GetByIdWithDetailsAsync(int id)
        => await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Include(l => l.Fine)
            .FirstOrDefaultAsync(l => l.Id == id);

    public async Task<IEnumerable<Loan>> GetByMemberIdAsync(int memberId)
        => await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Fine)
            .Where(l => l.MemberId == memberId)
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync();

    public async Task<IEnumerable<Loan>> GetByBookIdAsync(int bookId)
        => await _context.Loans
            .Include(l => l.Member)
            .Where(l => l.BookId == bookId)
            .OrderByDescending(l => l.LoanDate)
            .ToListAsync();

    public async Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status)
        => await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.Status == status)
            .ToListAsync();

    public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
        => await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.Status == LoanStatus.Active && l.DueDate < DateTime.UtcNow)
            .ToListAsync();

    public async Task<bool> HasActiveLoanForBookAsync(int memberId, int bookId)
        => await _context.Loans.AnyAsync(l =>
            l.MemberId == memberId &&
            l.BookId == bookId &&
            l.Status == LoanStatus.Active);
}
