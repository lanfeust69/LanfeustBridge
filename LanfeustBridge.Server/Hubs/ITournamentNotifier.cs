namespace LanfeustBridge.Hubs;

public interface ITournamentNotifier
{
    Task NewTournament();

    Task TournamentStarted(int tournamentId);

    Task TournamentFinished(int tournamentId);

    Task RoundFinished(int tournamentId, int round);

    Task NextRound(int tournamentId, int round);
}
