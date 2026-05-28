using EntrePaginas.Domain.Enums;

namespace EntrePaginas.API.DTOs.Request;

public class MemberRequestDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public MemberStatus Status { get; set; } = MemberStatus.Active;
}
