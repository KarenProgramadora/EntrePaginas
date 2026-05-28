using EntrePaginas.Domain.Entities;

namespace EntrePaginas.Domain.Interfaces.Services;

public interface IFineService
{
    Task<IEnumerable<Fine>> GetAllAsync();
    Task<Fine?> GetByIdAsync(int id);
    Task<Fine?> GetByLoanIdAsync(int loanId);
    Task<IEnumerable<Fine>> GetUnpaidFinesAsync();
    Task<IEnumerable<Fine>> GetByMemberIdAsync(int memberId);
    Task<Fine> CreateAsync(Fine fine);
    Task PayFineAsync(int id);
    Task DeleteAsync(int id);
}
