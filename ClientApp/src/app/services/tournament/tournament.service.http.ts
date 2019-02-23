import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, NgZone, PLATFORM_ID } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Observable, fromEvent, fromEventPattern } from 'rxjs';
import { filter, map } from 'rxjs/operators';

import { Tournament } from '../../tournament';
import { TournamentService } from './tournament.service';

@Injectable()
export class TournamentServiceHttp implements TournamentService {
    private _baseUrl: string;
    private _hubUrl: string;
    hubConnection: HubConnection;

    newTournamentObservable: Observable<void>;
    tournamentStartedObservable: Observable<number>;
    tournamentFinishedObservable: Observable<number>;

    constructor(private _http: HttpClient, private _ngZone: NgZone,
        @Inject('BASE_URL') originUrl: string,
        @Inject(PLATFORM_ID) platformId: Object) {
        this._baseUrl = originUrl + 'api/tournament';
        this._hubUrl = originUrl + 'hub/tournament';
        // no point in having signalR on server side (pre-rendering), which would require XMLHttpRequest
        if (isPlatformBrowser(platformId))
            this.initSignalR();
    }

    initSignalR() {
        this.hubConnection = new HubConnectionBuilder().withUrl(this._hubUrl).build();
        this.newTournamentObservable = this.wrapObservable(fromEvent(this.hubConnection, 'NewTournament').pipe(map((v, i) => void 0)));
        this.tournamentStartedObservable = this.wrapObservable(fromEvent(this.hubConnection, 'TournamentStarted'));
        this.tournamentFinishedObservable = this.wrapObservable(fromEvent(this.hubConnection, 'TournamentFinished'));
        // not so nice for e2e tests :
        // run outside angular so that protractor won't timeout waiting for tasks managed by signalR
        this._ngZone.runOutsideAngular(() =>
            this.hubConnection.start().then(() => console.log('hub connection started')));
    }

    getNames(): Observable<{ id: number; name: string }[]> {
        return this._http.get<{ id: number; name: string }[]>(this._baseUrl);
    }

    get(id: number): Observable<Tournament> {
        return this._http.get<Tournament>(this._baseUrl + '/' + id);
    }

    create(tournament: Tournament): Observable<Tournament> {
        return this._http.post<Tournament>(this._baseUrl, tournament);
    }

    update(tournament: Tournament): Observable<Tournament> {
        return this._http.put<Tournament>(this._baseUrl + '/' + tournament.id, tournament);
    }

    delete(id: number): Observable<boolean> {
        return this._http.delete<boolean>(this._baseUrl + '/' + id);
    }

    getScorings(): Observable<string[]> {
        return this._http.get<string[]>(this._baseUrl + '/scoring');
    }

    start(id: number): Observable<Tournament> {
        return this._http.post<Tournament>(this._baseUrl + '/' + id + '/start', '');
    }

    close(id: number): Observable<Tournament> {
        return this._http.post<Tournament>(this._baseUrl + '/' + id + '/close', '');
    }

    currentRound(id: number): Observable<{round: number, finished: boolean}> {
        return this._http.get<{round: number, finished: boolean}>(this._baseUrl + '/' + id + '/current-round');
    }

    nextRound(id: number) {
        // need to subscribe to actually post
        this._http.post(this._baseUrl + '/' + id + '/next-round', '', { observe: 'response' })
            .subscribe();
    }

    getNextRoundObservable(id: number): Observable<number> {
        return this.wrapObservable(fromEventPattern(
            handler => this.hubConnection.on('NextRound', handler as (...args: any[]) => void),
            handler => this.hubConnection.off('NextRound', handler as (...args: any[]) => void),
            (tournamentId, round) => ({tournamentId, round})).pipe(
                filter(({tournamentId, round}) => tournamentId === id),
                map(({tournamentId, round}) => round)));
    }

    getRoundFinishedObservable(id: number): Observable<number> {
        return this.wrapObservable(fromEventPattern(
            handler => this.hubConnection.on('RoundFinished', handler as (...args: any[]) => void),
            handler => this.hubConnection.off('RoundFinished', handler as (...args: any[]) => void),
            (tournamentId, round) => ({tournamentId, round})).pipe(
                filter(({tournamentId, round}) => tournamentId === id),
                map(({tournamentId, round}) => round)));
    }

    // we wrap observables created from signalR callbacks so that the subscribers run in angular zone
    private wrapObservable<T>(observable: Observable<T>): Observable<T> {
        const wrapped = Observable.create(subscriber => {
            const innerSub = observable.subscribe(
                t => this._ngZone.run(() => subscriber.next(t)),
                err => this._ngZone.run(() => subscriber.error(err)),
                () => this._ngZone.run(() => subscriber.complete())
            );
            return () => innerSub.unsubscribe();
        });
        return wrapped;
    }
}
