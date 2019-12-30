import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Suit } from '../../types';
import { Deal } from '../../deal';
import { Score } from '../../score';
import { DealService } from './deal.service';

@Injectable()
export class DealServiceHttp implements DealService {
    private _baseUrl: string;

    constructor(private _http: HttpClient, @Inject('BASE_URL') originUrl: string) {
        this._baseUrl = originUrl + 'api/tournament/';
    }

    getDeal(tournament: number, id: number): Observable<{deal: Deal, hasNext: boolean}> {
        return this._http.get<{deal: Deal, hasNext: boolean}>(this._baseUrl + tournament + '/deal/' + id);
    }

    getDeals(tournament: number): Observable<Deal[]> {
        return this._http.get<Deal[]>(this._baseUrl + tournament + '/deal');
    }

    getScore(tournament: number, id: number, round: number): Observable<Score> {
        return this._http.get<Score>(this._baseUrl + tournament + '/deal/' + id + '/score/' + round);
    }

    postScore(tournament: number, score: Score): Observable<Score> {
        // TODO : don't trust client side : compute on server
        score.score = Score.computeScore(score);
        return this._http.post<Score>(this._baseUrl + tournament + '/deal/' + score.dealId + '/score/' + score.round, score);
    }
}
