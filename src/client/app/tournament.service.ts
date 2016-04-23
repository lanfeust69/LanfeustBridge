import {OpaqueToken} from 'angular2/core';
import {Score} from './score';
import {Tournament} from './tournament';

export let TOURNAMENT_SERVICE = new OpaqueToken('TournamentService');

export interface TournamentService {
    getNames() : Promise<string[]>;
    get(name: string) : Promise<Tournament>;
    create(tournament: Tournament) : Promise<Tournament>;
    update(tournament: Tournament) : Promise<Tournament>;
    delete(name: string) : Promise<boolean>;

    // returns the tournament with positions filled
    start(name: string) : Promise<Tournament>;
    // returns the current ns and ew score filled 
    postScore(name: string, score: Score) : Promise<Score>;
    close(name: string) : Promise<Tournament>;
}
