import {Injectable} from 'angular2/core';
import {Tournament, Status} from './tournament';
import {Score} from './score';
import {TournamentService} from './tournament.service';

@Injectable()
export class TournamentServiceMock implements TournamentService {
    private _tournaments: Map<string, Tournament> = new Map<string, Tournament>();

    getNames() : Promise<string[]> {
        return Promise.resolve(Array.from(this._tournaments.keys()));
    }

    get(name: string) : Promise<Tournament> {
        if (!this._tournaments.has(name))
            return Promise.reject<Tournament>("No tournament named '" + name + "' found");
        let tournament = this._tournaments.get(name);
        //return Promise.resolve(tournament);
        return new Promise<Tournament>(resolve => setTimeout(() => resolve(tournament), 2000)); // 2 seconds
    }

    create(tournament: Tournament) : Promise<Tournament> {
        if (this._tournaments.has(tournament.name))
            return Promise.reject<Tournament>("A tournament named'" + tournament.name + "' already exists");
        this._tournaments.set(tournament.name, tournament);
        return Promise.resolve(tournament);
    }

    update(tournament: Tournament) : Promise<Tournament> {
        if (!this._tournaments.has(tournament.name))
            return Promise.reject<Tournament>("No tournament named '" + tournament.name + "' found");
        this._tournaments.set(tournament.name, tournament);
        return Promise.resolve(tournament);
    }

    delete(name: string) : Promise<boolean> {
        return Promise.resolve(this._tournaments.delete(name));
    }
    
    getMovements() : Promise<string[]> {
        return Promise.resolve(["Mitchell", "Howell", "Individual"]);
    }

    start(name: string) : Promise<Tournament> {
        if (!this._tournaments.has(name))
            return Promise.reject<Tournament>("No tournament named '" + name + "' found");
        let tournament = this._tournaments.get(name);
        tournament.status = Status.Running;
        // TODO : fill tournament.positions
        return Promise.resolve(tournament);        
    }

    postScore(name: string, score: Score) : Promise<Score> {
        if (!this._tournaments.has(name))
            return Promise.reject<Score>("No tournament named '" + name + "' found");
        let tournament = this._tournaments.get(name);
        tournament.deals[score.dealId - 1].scores[score.round - 1] = score;
        // TODO : update scores
        return Promise.resolve(score);
    }

    close(name: string) : Promise<Tournament> {
        if (!this._tournaments.has(name))
            return Promise.reject<Tournament>("No tournament named '" + name + "' found");
        let tournament = this._tournaments.get(name);
        tournament.status = Status.Finished;
        return Promise.resolve(tournament);
    }
}
