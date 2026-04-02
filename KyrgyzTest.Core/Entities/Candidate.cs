using KyrgyzTest.Core.Enums;
namespace KyrgyzTest.Core.Entities;


public class Candidate
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string ExamCode  { get; set; }
    public string PassportNumber { get; set; }
    public DateTime RegisteredAt { get; set; }
    public CandidateCategory Category { get; set; }
}