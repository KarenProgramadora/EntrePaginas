using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Repositories;
using EntrePaginas.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace EntrePaginas.Domain.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<MemberService> _logger;

    public MemberService(IMemberRepository memberRepository, ILogger<MemberService> logger)
    {
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all members");
        return await _memberRepository.GetAllAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving member with ID: {MemberId}", id);
        return await _memberRepository.GetByIdAsync(id);
    }

    public async Task<Member?> GetByIdWithLoansAsync(int id)
    {
        _logger.LogInformation("Retrieving member with loans, ID: {MemberId}", id);
        return await _memberRepository.GetByIdWithLoansAsync(id);
    }

    public async Task<Member> CreateAsync(Member member)
    {
        var existing = await _memberRepository.GetByEmailAsync(member.Email);
        if (existing != null)
        {
            _logger.LogWarning("Member with email '{Email}' already exists", member.Email);
            throw new InvalidOperationException($"Ya existe un miembro con el email '{member.Email}'");
        }

        member.MembershipDate = DateTime.UtcNow;
        _logger.LogInformation("Creating member: {Email}", member.Email);
        return await _memberRepository.CreateAsync(member);
    }

    public async Task UpdateAsync(int id, Member member)
    {
        var existing = await _memberRepository.GetByIdAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("Member with ID {MemberId} not found for update", id);
            throw new KeyNotFoundException($"No se encontró el miembro con ID {id}");
        }

        if (existing.Email != member.Email)
        {
            var withSameEmail = await _memberRepository.GetByEmailAsync(member.Email);
            if (withSameEmail != null)
                throw new InvalidOperationException($"Ya existe un miembro con el email '{member.Email}'");
        }

        existing.FirstName = member.FirstName;
        existing.LastName = member.LastName;
        existing.Email = member.Email;
        existing.Phone = member.Phone;
        existing.Status = member.Status;

        _logger.LogInformation("Updating member with ID: {MemberId}", id);
        await _memberRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        if (!await _memberRepository.ExistsAsync(id))
        {
            _logger.LogWarning("Member with ID {MemberId} not found for deletion", id);
            throw new KeyNotFoundException($"No se encontró el miembro con ID {id}");
        }

        _logger.LogInformation("Deleting member with ID: {MemberId}", id);
        await _memberRepository.DeleteAsync(id);
    }
}
