using EntrePaginas.Domain.Entities;

namespace EntrePaginas.Domain.Interfaces.Repositories;

public interface IMemberRepository : IGenericRepository<Member>
{
    Task<Member?> GetByEmailAsync(string email);
    Task<Member?> GetByIdWithLoansAsync(int id);
}
