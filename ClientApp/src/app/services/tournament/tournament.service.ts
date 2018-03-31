import {InjectionToken} from '@angular/core';
import {Observable} from 'rxjs/Rx';
import {Tournament} from '../../tournament';

export let TOURNAMENT_SERVICE = new InjectionToken('TournamentService');

export interface TournamentService {
    getNames(): Observable<{id: number; name: string}[]>;
    get(id: number): Observable<Tournament>;
    create(tournament: Tournament): Observable<Tournament>;
    update(tournament: Tournament): Observable<Tournament>;
    delete(id: number): Observable<boolean>;

    getScorings(): Observable<string[]>;

    // returns the tournament with positions filled
    start(id: number): Observable<Tournament>;
    close(id: number): Observable<Tournament>;

    currentRound(id: number): Observable<{round: number, finished: boolean}>;
    nextRound(id: number): void;

    newTournamentObservable: Observable<void>;
    tournamentStartedObservable: Observable<number>;
    tournamentFinishedObservable: Observable<number>;
    getNextRoundObservable(id: number): Observable<number>;
    getRoundFinishedObservable(id: number): Observable<number>;
}
