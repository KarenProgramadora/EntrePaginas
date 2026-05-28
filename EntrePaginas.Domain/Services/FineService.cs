using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Repositories;
using EntrePaginas.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EntrePaginas.Domain.Services;

public class FineService : IFineService
{
    private readonly IFineRepository _fineRepository;
    private readonly ILogger<FineService> _logger;

    public FineService(IFineRepository fineRepository, ILogger<FineService> logger)
    {
        _fineRepository = fineRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Fine>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all fines");
        return await _fineRepository.GetAllAsync();
    }

    public async Task<Fine?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving fine with ID: {FineId}", id);
        return await _fineRepository.GetByIdAsync(id);
    }

    public async Task<Fine?> GetByLoanIdAsync(int loanId)
    {
        _logger.LogInformation("Retrieving fine for loan ID: {LoanId}", loanId);
        return await _fineRepository.GetByLoanIdAsync(loanId);
    }

    public async Task<IEnumerable<Fine>> GetUnpaidFinesAsync()
    {
        _logger.LogInformation("Retrieving unpaid fines");
        return await _fineRepository.GetUnpaidFinesAsync();
    }

    public async Task<IEnumerable<Fine>> GetByMemberIdAsync(int memberId)
    {
        _logger.LogInformation("Retrieving fines for member ID: {MemberId}", memberId);
        return await _fineRepository.GetByMemberIdAsync(memberId);
    }

    public async Task<Fine> CreateAsync(Fine fine)
    {
        var existing = await _fineRepository.GetByLoanIdAsync(fine.LoanId);
        if (existing != null)
            throw new InvalidOperationException($"Ya existe una multa para el préstamo con ID {fine.LoanId}");

        if (fine.Amount <= 0)
            throw new InvalidOperationException("El monto de la multa debe ser mayor a cero");

        fine.IssuedDate = DateTime.UtcNow;
        fine.IsPaid = false;

        _logger.LogInformation("Creating fine for loan ID: {LoanId}", fine.LoanId);
        return await _fineRepository.CreateAsync(fine);
    }

    public async Task PayFineAsync(int id)
    {
        var fine = await _fineRepository.GetByIdAsync(id);
        if (fine == null)
        {
            _logger.LogWarning("Fine with ID {FineId} not found", id);
            throw new KeyNotFoundException($"No se encontró la multa con ID {id}");
        }

        if (fine.IsPaid)
            throw new InvalidOperationException($"La multa con ID {id} ya fue pagada");

        fine.IsPaid = true;
        fine.PaidDate = DateTime.UtcNow;

        _logger.LogInformation("Paying fine with ID: {FineId}", id);
        await _fineRepository.UpdateAsync(fine);
    }

    public async Task DeleteAsync(int id)
    {
        if (!await _fineRepository.ExistsAsync(id))
        {
            _logger.LogWarning("Fine with ID {FineId} not found for deletion", id);
            throw new KeyNotFoundException($"No se encontró la multa con ID {id}");
        }

        _logger.LogInformation("Deleting fine with ID: {FineId}", id);
        await _fineRepository.DeleteAsync(id);
    }
}
