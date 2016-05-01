import {Injectable} from 'angular2/core';
import {Http, Headers, RequestOptions, Response} from 'angular2/http';
import {Tournament, Status} from './tournament';
import {Score} from './score';
import {TournamentService} from './tournament.service';

import 'rxjs/Rx';

@Injectable()
export class TournamentServiceHttp implements TournamentService {
    private _tournaments: Tournament[] = [];
    private _baseUrl = 'api/tournament';

    constructor(private _http: Http) {}

    getNames(): Promise<{ id: number; name: string }[]> {
        return this._http.get(this._baseUrl).map(this.extractData).toPromise();
    }

    private extractData(res: Response) {
        if (res.status < 200 || res.status >= 300) {
            throw new Error('Bad response status: ' + res.status);
        }
        let data = res.json();
        if (data.date)
            data.date = new Date(data.date);
        return data;
    }

    get(id: number) : Promise<Tournament> {
        return this._http.get(this._baseUrl + '/' + id).map(this.extractData).toPromise();
    }

    create(tournament: Tournament): Promise<Tournament> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.post(this._baseUrl, JSON.stringify(tournament), options).map(this.extractData).toPromise();
    }

    update(tournament: Tournament) : Promise<Tournament> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.put(this._baseUrl + '/' + tournament.id, JSON.stringify(tournament), options).map(this.extractData).toPromise();
    }

    delete(id: number) : Promise<boolean> {
        return this._http.delete(this._baseUrl + '/' + id).map(this.extractData).toPromise();
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

    close(id: number) : Promise<Tournament> {
        if (id < 0 || id >= this._tournaments.length || !this._tournaments[id]) 
            return Promise.reject<Tournament>("No tournament with id '" + id + "' found");
        let tournament = this._tournaments[id];
        tournament.status = Status.Finished;
        return Promise.resolve(tournament);
    }
}
