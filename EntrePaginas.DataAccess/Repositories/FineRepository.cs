using EntrePaginas.DataAccess.Context;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EntrePaginas.DataAccess.Repositories;

public class FineRepository : GenericRepository<Fine>, IFineRepository
{
    public FineRepository(EntrePaginasDbContext context) : base(context) { }

    public async Task<Fine?> GetByLoanIdAsync(int loanId)
        => await _context.Fines
            .Include(f => f.Loan)
            .FirstOrDefaultAsync(f => f.LoanId == loanId);

    public async Task<IEnumerable<Fine>> GetUnpaidFinesAsync()
        => await _context.Fines
            .Include(f => f.Loan)
                .ThenInclude(l => l.Member)
            .Where(f => !f.IsPaid)
            .ToListAsync();

    public async Task<IEnumerable<Fine>> GetByMemberIdAsync(int memberId)
        => await _context.Fines
            .Include(f => f.Loan)
            .Where(f => f.Loan.MemberId == memberId)
            .ToListAsync();
}
