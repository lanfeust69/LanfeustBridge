using LanfeustBridge.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace LanfeustBridge.Hubs
{
    public class TournamentHub : Hub<ITournamentNotifier>
    {
    }
}
