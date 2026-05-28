using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Enums;

namespace EntrePaginas.Domain.Interfaces.Repositories;

public interface ILoanRepository : IGenericRepository<Loan>
{
    Task<Loan?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Loan>> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<Loan>> GetByBookIdAsync(int bookId);
    Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status);
    Task<IEnumerable<Loan>> GetOverdueLoansAsync();
    Task<bool> HasActiveLoanForBookAsync(int memberId, int bookId);
}
