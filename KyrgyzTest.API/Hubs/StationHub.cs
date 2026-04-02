using Microsoft.AspNetCore.SignalR;

namespace KyrgyzTest.API.Hubs;

public class StationHub: Hub
{
    public async Task RegisterStation(int stationNumber)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"station-{stationNumber}");
        
        await Clients.Caller.SendAsync("StationRegistered", stationNumber);
    }

    public async Task OpenExam(int stationNumber, string examCode)
    {
        await Clients.Group($"station-{stationNumber}").SendAsync("ExamOpen", examCode);
    }
}