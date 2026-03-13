using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Entities;

public class Computer
{
    public Guid Id { get; set; }
    public int StationNumber { get; set; }
    public ComputerStatus Status {get; set; }
    public DateTime LastHeartbeat { get; set; }
}