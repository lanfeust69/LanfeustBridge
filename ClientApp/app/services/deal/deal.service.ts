import {OpaqueToken} from '@angular/core';
import {Score} from '../../score';
import {Deal} from '../../deal';

export let DEAL_SERVICE = new OpaqueToken('DealService');

export interface DealService {
    getDeal(tournament: number, id: number) : Promise<Deal>;
    getDeals(tournament: number) : Promise<Deal[]>;

    // returns the current ns and ew score filled 
    getScore(tournament: number, id: number, round: number) : Promise<Score>;
    postScore(id: number, score: Score) : Promise<Score>;
}
