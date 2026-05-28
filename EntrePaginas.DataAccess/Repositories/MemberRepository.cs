using EntrePaginas.DataAccess.Context;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EntrePaginas.DataAccess.Repositories;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{
    public MemberRepository(EntrePaginasDbContext context) : base(context) { }

    public async Task<Member?> GetByEmailAsync(string email)
        => await _context.Members.FirstOrDefaultAsync(m => m.Email == email);

    public async Task<Member?> GetByIdWithLoansAsync(int id)
        => await _context.Members
            .Include(m => m.Loans)
                .ThenInclude(l => l.Book)
            .FirstOrDefaultAsync(m => m.Id == id);
}
