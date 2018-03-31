import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, NgZone, PLATFORM_ID } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { HttpConnection, HubConnection } from '@aspnet/signalr';
import { Observable } from 'rxjs/Observable';

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

    constructor(private _http: Http, private _ngZone: NgZone,
        @Inject('BASE_URL') originUrl: string,
        @Inject(PLATFORM_ID) platformId: Object) {
        this._baseUrl = originUrl + 'api/tournament';
        this._hubUrl = originUrl + 'hub/tournament';
        // no point in having signalR on server side (pre-rendering), which would require XMLHttpRequest
        if (isPlatformBrowser(platformId))
            this.initSignalR();
    }

    initSignalR() {
        const cnx = new HttpConnection(this._hubUrl);
        this.hubConnection = new HubConnection(cnx);
        this.newTournamentObservable = this.wrapObservable(Observable.fromEvent(this.hubConnection, 'NewTournament').map((v, i) => void 0));
        this.tournamentStartedObservable = this.wrapObservable(Observable.fromEvent(this.hubConnection, 'TournamentStarted'));
        this.tournamentFinishedObservable = this.wrapObservable(Observable.fromEvent(this.hubConnection, 'TournamentFinished'));
        // not so nice for e2e tests :
        // run outside angular so that protractor won't timeout waiting for tasks managed by signalR
        this._ngZone.runOutsideAngular(() =>
            this.hubConnection.start().then(() => console.log('hub connection started')));
    }

    getNames(): Observable<{ id: number; name: string }[]> {
        return this._http.get(this._baseUrl).map(this.extractData);
    }

    get(id: number): Observable<Tournament> {
        return this._http.get(this._baseUrl + '/' + id).map(this.extractData);
    }

    create(tournament: Tournament): Observable<Tournament> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        return this._http.post(this._baseUrl, JSON.stringify(tournament), options).map(this.extractData);
    }

    update(tournament: Tournament): Observable<Tournament> {
        const headers = new Headers({ 'Content-Type': 'application/json' });
        const options = new RequestOptions({ headers: headers });
        return this._http.put(this._baseUrl + '/' + tournament.id, JSON.stringify(tournament), options).map(this.extractData);
    }

    delete(id: number): Observable<boolean> {
        return this._http.delete(this._baseUrl + '/' + id).map(this.extractData);
    }

    getScorings(): Observable<string[]> {
        return this._http.get(this._baseUrl + '/scoring').map(this.extractData);
    }

    start(id: number): Observable<Tournament> {
        return this._http.post(this._baseUrl + '/' + id + '/start', '').map(this.extractData);
    }

    close(id: number): Observable<Tournament> {
        return this._http.post(this._baseUrl + '/' + id + '/close', '').map(this.extractData);
    }

    currentRound(id: number): Observable<{round: number, finished: boolean}> {
        return this._http.get(this._baseUrl + '/' + id + '/current-round').map(this.extractData);
    }

    nextRound(id: number) {
        // need to subscribe to actually post
        this._http.post(this._baseUrl + '/' + id + '/next-round', '')
            .subscribe(r => console.log('Next Round POST returned ' + r.statusText));
    }

    getNextRoundObservable(id: number): Observable<number> {
        return this.wrapObservable(Observable.fromEventPattern(
            handler => this.hubConnection.on('NextRound', handler as (...args: any[]) => void),
            handler => this.hubConnection.off('NextRound', handler as (...args: any[]) => void),
            (tournamentId, round) => ({tournamentId, round}))
            .filter(({tournamentId, round}) => tournamentId === id)
            .map(({tournamentId, round}) => round));
    }

    getRoundFinishedObservable(id: number): Observable<number> {
        return this.wrapObservable(Observable.fromEventPattern(
            handler => this.hubConnection.on('RoundFinished', handler as (...args: any[]) => void),
            handler => this.hubConnection.off('RoundFinished', handler as (...args: any[]) => void),
            (tournamentId, round) => ({tournamentId, round}))
            .filter(({tournamentId, round}) => tournamentId === id)
            .map(({tournamentId, round}) => round));
    }

    private extractData(res: Response) {
        if (res.status < 200 || res.status >= 300) {
            throw new Error('Bad response status: ' + res.status);
        }
        const data = res.json();
        if (data.date)
            data.date = new Date(data.date);
        return data;
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
