import {Injectable} from '@angular/core';
import {Http, Headers, RequestOptions, Response} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {Tournament} from '../../tournament';
import {TournamentService} from './tournament.service';

@Injectable()
export class TournamentServiceHttp implements TournamentService {
    private _baseUrl = 'api/tournament';

    constructor(private _http: Http) {}

    getNames(): Observable<{ id: number; name: string }[]> {
        return this._http.get(this._baseUrl).map(this.extractData);
    }

    get(id: number): Observable<Tournament> {
        return this._http.get(this._baseUrl + '/' + id).map(this.extractData);
    }

    create(tournament: Tournament): Observable<Tournament> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.post(this._baseUrl, JSON.stringify(tournament), options).map(this.extractData);
    }

    update(tournament: Tournament): Observable<Tournament> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.put(this._baseUrl + '/' + tournament.id, JSON.stringify(tournament), options).map(this.extractData);
    }

    delete(id: number): Observable<boolean> {
        return this._http.delete(this._baseUrl + '/' + id).map(this.extractData);
    }

    getScorings(): Observable<string[]> {
        return this._http.get(this._baseUrl + '/scoring').map(this.extractData);
    }

    start(id: number): Observable<Tournament> {
        return this._http.post(this._baseUrl + '/' + id + '/start', '').map(this.extractData);
    }

    close(id: number): Observable<Tournament> {
        return this._http.post(this._baseUrl + '/' + id + '/close', '').map(this.extractData);
    }

    currentRound(id: number): Observable<{round: number, finished: boolean}> {
        return this._http.get(this._baseUrl + '/' + id + '/current-round').map(this.extractData);
    }

    nextRound(id: number) {
        // need to subscribe to actually post
        this._http.post(this._baseUrl + '/' + id + '/next-round', '')
            .subscribe(r => console.log('Next Round POST returned ' + r.statusText));
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
}
