using EntrePaginas.Domain.Entities;

namespace EntrePaginas.Domain.Interfaces.Services;

public interface ILoanService
{
    Task<IEnumerable<Loan>> GetAllAsync();
    Task<Loan?> GetByIdAsync(int id);
    Task<Loan?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Loan>> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<Loan>> GetOverdueLoansAsync();
    Task<Loan> CreateLoanAsync(int memberId, int bookId, int loanDays);
    Task ReturnLoanAsync(int id);
    Task MarkOverdueLoansAsync();
}
