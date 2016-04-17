import {Injectable} from 'angular2/core';
import {Tournament} from './tournament';
import {TournamentService} from './tournament.service';

@Injectable()
export class TournamentServiceMock implements TournamentService {
    private _tournaments: Map<string, Tournament>;
    getTournament(tournamentName: string) : Promise<Tournament> {
        if (!this._tournaments.has(tournamentName))
            return Promise.reject<Tournament>("No tournament named '" + tournamentName + "' found");
        let tournament = this._tournaments[tournamentName];
        //return Promise.resolve(tournament);
        return new Promise<Tournament>(resolve => setTimeout(() => resolve(tournament), 2000)); // 2 seconds
    }

    createTournament(tournament: Tournament) : Promise<Tournament> {
        if (this._tournaments.has(tournament.name))
            return Promise.reject<Tournament>("A tournament named'" + tournament.name + "' already exists");
        this._tournaments.set(tournament.name, tournament);
        return Promise.resolve(tournament);
    }

    updateTournament(tournament: Tournament) : Promise<Tournament> {
        if (!this._tournaments.has(tournament.name))
            return Promise.reject<Tournament>("No tournament named '" + tournament.name + "' found");
        this._tournaments.set(tournament.name, tournament);
        return Promise.resolve(tournament);
    }
}
