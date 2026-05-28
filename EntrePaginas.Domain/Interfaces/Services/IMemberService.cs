using EntrePaginas.Domain.Entities;

namespace EntrePaginas.Domain.Interfaces.Services;

public interface IMemberService
{
    Task<IEnumerable<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task<Member?> GetByIdWithLoansAsync(int id);
    Task<Member> CreateAsync(Member member);
    Task UpdateAsync(int id, Member member);
    Task DeleteAsync(int id);
}
