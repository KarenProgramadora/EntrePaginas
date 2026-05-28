using EntrePaginas.Domain.Entities;

namespace EntrePaginas.Domain.Interfaces.Repositories;

public interface IFineRepository : IGenericRepository<Fine>
{
    Task<Fine?> GetByLoanIdAsync(int loanId);
    Task<IEnumerable<Fine>> GetUnpaidFinesAsync();
    Task<IEnumerable<Fine>> GetByMemberIdAsync(int memberId);
}
