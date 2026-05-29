using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Enums;
using EntrePaginas.Domain.Helper;
using EntrePaginas.Domain.Interfaces.Repositories;
using EntrePaginas.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EntrePaginas.Domain.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly LoanValidationHelper _validationHelper;
    private readonly ILogger<LoanService> _logger;

    public LoanService(
        ILoanRepository loanRepository,
        IBookRepository bookRepository,
        LoanValidationHelper validationHelper,
        ILogger<LoanService> logger)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _validationHelper = validationHelper;
        _logger = logger;
    }

    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all loans");
        return await _loanRepository.GetAllAsync();
    }

    public async Task<Loan?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving loan with ID: {LoanId}", id);
        return await _loanRepository.GetByIdAsync(id);
    }

    public async Task<Loan?> GetByIdWithDetailsAsync(int id)
    {
        _logger.LogInformation("Retrieving loan with details, ID: {LoanId}", id);
        return await _loanRepository.GetByIdWithDetailsAsync(id);
    }

    public async Task<IEnumerable<Loan>> GetByMemberIdAsync(int memberId)
    {
        _logger.LogInformation("Retrieving loans for member ID: {MemberId}", memberId);
        return await _loanRepository.GetByMemberIdAsync(memberId);
    }

    public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
    {
        _logger.LogInformation("Retrieving overdue loans");
        return await _loanRepository.GetOverdueLoansAsync();
    }

    public async Task<Loan> CreateLoanAsync(int memberId, int bookId, int loanDays)
    {
        // Validaciones delegadas al helper (patrón análogo a MatchValidationHelper)
        LoanValidationHelper.ValidateLoanDays(loanDays);
        await _validationHelper.ValidateActiveMemberAsync(memberId);
        var book = await _validationHelper.ValidateAvailableBookAsync(bookId);
        await _validationHelper.ValidateNoDuplicateActiveLoanAsync(memberId, bookId, book.Title);

        var loan = new Loan
        {
            MemberId = memberId,
            BookId = bookId,
            LoanDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(loanDays),
            Status = LoanStatus.Active
        };

        // Descontar copia disponible
        book.AvailableCopies--;
        await _bookRepository.UpdateAsync(book);

        _logger.LogInformation("Creating loan: member {MemberId}, book {BookId}, due {DueDate}",
            memberId, bookId, loan.DueDate);

        return await _loanRepository.CreateAsync(loan);
    }

    public async Task ReturnLoanAsync(int id)
    {
        var loan = await _loanRepository.GetByIdWithDetailsAsync(id);
        if (loan == null)
        {
            _logger.LogWarning("Loan with ID {LoanId} not found for return", id);
            throw new KeyNotFoundException($"No se encontró el préstamo con ID {id}");
        }

        // Máquina de estados: solo se puede devolver un préstamo Active u Overdue
        if (loan.Status == LoanStatus.Returned)
            throw new InvalidOperationException($"El préstamo con ID {id} ya fue devuelto");

        loan.ReturnDate = DateTime.UtcNow;
        loan.Status = LoanStatus.Returned;

        // Restaurar copia disponible
        var book = await _bookRepository.GetByIdAsync(loan.BookId);
        if (book != null)
        {
            book.AvailableCopies++;
            await _bookRepository.UpdateAsync(book);
        }

        _logger.LogInformation("Returning loan with ID: {LoanId}", id);
        await _loanRepository.UpdateAsync(loan);
    }

    public async Task MarkOverdueLoansAsync()
    {
        var overdueLoans = await _loanRepository.GetOverdueLoansAsync();
        int count = 0;

        foreach (var loan in overdueLoans)
        {
            loan.Status = LoanStatus.Overdue;
            await _loanRepository.UpdateAsync(loan);
            count++;
        }

        _logger.LogInformation("Marked {Count} loans as overdue", count);
    }
}
