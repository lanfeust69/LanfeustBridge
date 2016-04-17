import {OpaqueToken} from 'angular2/core';
import {Deal} from './deal';

export let DEAL_SERVICE = new OpaqueToken('DealService');

export interface DealService {
    getDeal(tournament: string, id: number) : Promise<Deal>;
}
