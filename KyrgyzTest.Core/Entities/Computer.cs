using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Entities;

public class Computer
{
    public Guid Id { get; set; }
    public int StationNumber { get; set; }
    public ComputerStatus Status {get; set; }
    public DateTime LastHeartbeat { get; set; }
    
    // -- Security -- 
    public string DeviceId { get; set; } = string.Empty;
    public string DeviceSecretHash { get; set; } = string.Empty;
    public bool IsTrusted { get; set; } = false;
    public string? LastIpAddress { get; set; }
}