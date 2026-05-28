namespace EntrePaginas.API.DTOs.Response;

public class LoanResponseDTO
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public int MemberId { get; set; }
    public string MemberFullName { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public bool IsOverdue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
