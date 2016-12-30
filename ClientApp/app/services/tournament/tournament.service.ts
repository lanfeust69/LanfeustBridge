import {OpaqueToken} from '@angular/core';
import {Observable} from 'rxjs/Rx';
import {Score} from '../../score';
import {Tournament} from '../../tournament';

export let TOURNAMENT_SERVICE = new OpaqueToken('TournamentService');

export interface TournamentService {
    getNames() : Observable<{id: number; name: string}[]>;
    get(id: number) : Observable<Tournament>;
    create(tournament: Tournament) : Observable<Tournament>;
    update(tournament: Tournament) : Observable<Tournament>;
    delete(id: number) : Observable<boolean>;
    
    getMovements() : Observable<{name: string, nbTables: number}[]>;
    getScorings() : Observable<string[]>;

    // returns the tournament with positions filled
    start(id: number) : Observable<Tournament>;
    close(id: number) : Observable<Tournament>;

    currentRound(id: number) : Observable<{round: number, finished: boolean}>;
    nextRound(id: number) : void;
}
