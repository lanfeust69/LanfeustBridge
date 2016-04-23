import {Injectable} from 'angular2/core';
import {Tournament, Status} from './tournament';
import {Score} from './score';
import {TournamentService} from './tournament.service';

@Injectable()
export class TournamentServiceMock implements TournamentService {
    private _tournaments: Tournament[] = [];

    getNames() : Promise<{id: number; name: string}[]> {
        let result = this._tournaments.map((value, i) => ({id: i, name: value.name}));
        return Promise.resolve(result);
    }

    get(id: number) : Promise<Tournament> {
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<Tournament>("No tournament with id '" + id + "' found");
        let tournament = this._tournaments[id];
        //return Promise.resolve(tournament);
        return new Promise<Tournament>(resolve => setTimeout(() => resolve(tournament), 2000)); // 2 seconds
    }

    create(tournament: Tournament) : Promise<Tournament> {
        tournament.id = this._tournaments.length;
        this._tournaments.push(tournament);
        //return Promise.resolve(tournament);
        return new Promise<Tournament>(resolve => setTimeout(() => resolve(tournament), 2000)); // 2 seconds
    }

    update(tournament: Tournament) : Promise<Tournament> {
        let id = tournament.id;
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<Tournament>("No tournament with id '" + id + "' found");
        this._tournaments[id] = tournament;
        return Promise.resolve(tournament);
    }

    delete(id: number) : Promise<boolean> {
        if (id >= 0 && id < this._tournaments.length && this._tournaments[id]) {
            this._tournaments[id] = undefined;
            return Promise.resolve(true);
        }
        return Promise.resolve(false);
    }
    
    getMovements() : Promise<string[]> {
        return Promise.resolve(["Mitchell", "Howell", "Individual"]);
    }

    start(id: number) : Promise<Tournament> {
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<Tournament>("No tournament with id '" + id + "' found");
        let tournament = this._tournaments[id];
        tournament.status = Status.Running;
        // TODO : fill tournament.positions
        return Promise.resolve(tournament);        
    }

    postScore(id: number, score: Score) : Promise<Score> {
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<Score>("No tournament with id '" + id + "' found");
        let tournament = this._tournaments[id];
        tournament.deals[score.dealId - 1].scores[score.round - 1] = score;
        // TODO : update scores
        return Promise.resolve(score);
    }

    close(id: number) : Promise<Tournament> {
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<Tournament>("No tournament with id '" + id + "' found");
        let tournament = this._tournaments[id];
        tournament.status = Status.Finished;
        return Promise.resolve(tournament);
    }
}
