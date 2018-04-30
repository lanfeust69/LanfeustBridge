namespace LanfeustBridge.Hubs
{
    public interface ITournamentNotifier
    {
        void NewTournament();

        void TournamentStarted(int tournamentId);

        void TournamentFinished(int tournamentId);

        void RoundFinished(int tournamentId, int round);

        void NextRound(int tournamentId, int round);
    }
}
