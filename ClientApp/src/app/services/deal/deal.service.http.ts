import { Inject, Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { Suit } from '../../types';
import { Deal } from '../../deal';
import { Score } from '../../score';
import { DealService } from './deal.service';

@Injectable()
export class DealServiceHttp implements DealService {
    private _baseUrl: string;

    constructor(private _http: Http, @Inject('BASE_URL') originUrl: string) {
        this._baseUrl = originUrl + 'api/tournament/';
    }

    private extractData(res: Response) {
        if (res.status < 200 || res.status >= 300) {
            throw new Error('Bad response status: ' + res.status);
        }
        return res.json();
    }

    getDeal(tournament: number, id: number): Observable<Deal> {
        return this._http.get(this._baseUrl + tournament + '/deal/' + id).map(this.extractData);
    }

    getDeals(tournament: number): Observable<Deal[]> {
        return this._http.get(this._baseUrl + tournament + '/deal').map(this.extractData);
    }

    getScore(tournament: number, id: number, round: number): Observable<Score> {
        return this._http.get(this._baseUrl + tournament + '/deal/' + id + '/score/' + round).map(this.extractData);
    }

    postScore(tournament: number, score: Score): Observable<Score> {
        // TODO : don't trust client side : compute on server
        score.score = Score.computeScore(score);
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        return this._http.post(this._baseUrl + tournament + '/deal/' + score.dealId + '/score/' + score.round,
            JSON.stringify(score), options).map(this.extractData);
    }
}
