import {Injectable} from 'angular2/core';
import {Http, Headers, RequestOptions, Response} from 'angular2/http';
import {Suit} from './types';
import {Deal} from './deal';
import {Score} from './score';
import {DealService} from './deal.service';

@Injectable()
export class DealServiceHttp implements DealService {
    private _baseUrl = 'api/tournament/';

    constructor(private _http: Http) {}

    private extractData(res: Response) {
        if (res.status < 200 || res.status >= 300) {
            throw new Error('Bad response status: ' + res.status);
        }
        return res.json();
    }

    getDeal(tournament: number, id: number) : Promise<Deal> {
        return this._http.get(this._baseUrl + tournament + '/deal/' + id).map(this.extractData).toPromise();
    }

    getScore(tournament: number, id: number, round: number) : Promise<Score> {
        return this._http.get(this._baseUrl + tournament + '/deal/' + id + '/score/' + round).map(this.extractData).toPromise();
    }

    postScore(tournament: number, score: Score) : Promise<Score> {
        // TODO : don't trust client side : compute on server
        score.score = Score.computeScore(score);
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this._http.post(this._baseUrl + tournament + '/deal/' + score.dealId + '/score/' + score.round, JSON.stringify(score), options).map(this.extractData).toPromise();
    }
}
