import {OpaqueToken} from 'angular2/core';
import {Score} from './score';
import {Tournament} from './tournament';

export let TOURNAMENT_SERVICE = new OpaqueToken('TournamentService');

export interface TournamentService {
    getNames() : Promise<{id: number; name: string}[]>;
    get(id: number) : Promise<Tournament>;
    create(tournament: Tournament) : Promise<Tournament>;
    update(tournament: Tournament) : Promise<Tournament>;
    delete(id: number) : Promise<boolean>;
    
    getMovements() : Promise<string[]>;

    // returns the tournament with positions filled
    start(id: number) : Promise<Tournament>;
    close(id: number) : Promise<Tournament>;
}
