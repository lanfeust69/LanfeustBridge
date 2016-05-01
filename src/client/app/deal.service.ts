import {OpaqueToken} from 'angular2/core';
import {Score} from './score';
import {Deal} from './deal';

export let DEAL_SERVICE = new OpaqueToken('DealService');

export interface DealService {
    getDeal(tournament: number, id: number) : Promise<Deal>;

    // returns the current ns and ew score filled 
    postScore(id: number, score: Score) : Promise<Score>;
}
