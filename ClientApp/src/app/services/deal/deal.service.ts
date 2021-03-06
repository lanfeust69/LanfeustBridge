import { InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';

import { Score } from '../../score';
import { Deal } from '../../deal';

export const DEAL_SERVICE = new InjectionToken('DealService');

export interface DealService {
    getDeal(tournament: number, id: number): Observable<{deal: Deal, hasNext: boolean}>;
    getDeals(tournament: number): Observable<Deal[]>;

    // returns the current ns and ew score filled
    getScore(tournament: number, id: number, round: number, table: number): Observable<Score>;
    postScore(id: number, score: Score): Observable<Score>;
}
