import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Tournament, Position, Status } from '../../tournament';
import { Score } from '../../score';
import { DealService, DEAL_SERVICE } from '../deal/deal.service';
import { TournamentService } from './tournament.service';

@Injectable()
export class TournamentServiceMock implements TournamentService {
    private _tournaments: Tournament[] = [];

    newTournamentObservable: Observable<void>;
    tournamentStartedObservable: Observable<number>;
    tournamentFinishedObservable: Observable<number>;

    constructor(@Inject(DEAL_SERVICE) private _dealService: DealService) {}

    getNames(): Observable<{id: number; name: string}[]> {
        const result = this._tournaments.map((value, i) => ({id: i + 1, name: value.name}));
        return Observable.of(result);
    }

    get(id: number): Observable<Tournament> {
        if (id <= 0 || id > this._tournaments.length || !this._tournaments[id - 1])
            return Observable.throw('No tournament with id \'' + id + '\' found');
        const tournament = this._tournaments[id - 1];
        return Observable.of(tournament).delay(2000); // 2 seconds
    }

    create(tournament: Tournament): Observable<Tournament> {
        tournament.id = this._tournaments.length + 1;
        const allPositions: Position[][] = [];
        // mock : mitchell
        const nbDeals = tournament.nbTables * tournament.nbDealsPerRound;
        for (let r = 0; r < tournament.nbRounds; r++) {
            const positions: Position[] = [];
            for (let t = 0; t < tournament.nbTables; t++) {
                for (let i = 0; i < 4; i++) {
                    const position = new Position;
                    position.table = t + 1;
                    position.north = t * 4;
                    position.south = t * 4 + 1;
                    position.east = t * 4 + 2;
                    position.west = t * 4 + 3;
                    position.deals = [];
                    for (let d = 0; d < tournament.nbDealsPerRound; d++) {
                        position.deals.push(((t + r) * tournament.nbDealsPerRound) % nbDeals + d + 1);
                    }
                    positions.push(position);
                }
            }
            allPositions.push(positions);
        }
        tournament.positions = allPositions;
        // not always true, but OK for a mock
        tournament.nbDeals = tournament.nbDealsPerRound * tournament.nbRounds;
        this._tournaments.push(tournament);
        return Observable.of(tournament).delay(400); // 0.4 seconds
    }

    update(tournament: Tournament): Observable<Tournament> {
        const id = tournament.id;
        if (id <= 0 || id > this._tournaments.length || !this._tournaments[id - 1])
            return Observable.throw('No tournament with id \'' + id + '\' found');
        this._tournaments[id - 1] = tournament;
        return Observable.of(tournament);
    }

    delete(id: number): Observable<boolean> {
        if (id > 0 && id <= this._tournaments.length && this._tournaments[id - 1]) {
            this._tournaments[id - 1] = undefined;
            return Observable.of(true);
        }
        return Observable.of(false);
    }

    getScorings(): Observable<string[]> {
        return Observable.of(['Matchpoint', 'IMP']);
    }

    start(id: number): Observable<Tournament> {
        if (id <= 0 || id > this._tournaments.length || !this._tournaments[id - 1])
            return Observable.throw('No tournament with id "' + id + '" found');
        const tournament = this._tournaments[id - 1];
        tournament.status = Status.Running;
        tournament.currentRound = 0;
        return Observable.of(tournament);
    }

    close(id: number): Observable<Tournament> {
        if (id <= 0 || id > this._tournaments.length || !this._tournaments[id - 1])
            return Observable.throw('No tournament with id "' + id + '" found');
        const tournament = this._tournaments[id - 1];
        tournament.status = Status.Finished;
        return Observable.of(tournament);
    }

    currentRound(id: number): Observable<{round: number, finished: boolean}> {
        if (id <= 0 || id > this._tournaments.length || !this._tournaments[id - 1])
            return Observable.throw('No tournament with id "' + id + '" found');
        const tournament = this._tournaments[id - 1];
        // finished, waiting for close
        if (tournament.currentRound === tournament.nbRounds)
            return Observable.of({round: tournament.currentRound, finished: true});
        // only in mock : assume all deals are played each round
        let scores: Observable<Score>;
        for (let dealId = 1; dealId <= tournament.nbDealsPerRound * tournament.nbTables; dealId++) {
            const score = this._dealService.getScore(id, dealId, tournament.currentRound);
            scores = scores ? scores.merge(score) : score;
        }
        return scores
            .reduce((allEntered: boolean, score: Score, _) => allEntered && score.entered, true)
            .map(b => ({round: tournament.currentRound, finished: b}));
    }

    nextRound(id: number) {
        if (id <= 0 || id > this._tournaments.length || !this._tournaments[id - 1])
            return Observable.throw('No tournament with id "' + id + '" found');
        const tournament = this._tournaments[id - 1];
        if (tournament.currentRound < tournament.nbRounds)
            tournament.currentRound++;
    }

    getNextRoundObservable(id: number): Observable<number> {
        return Observable.of(0);
    }

    getRoundFinishedObservable(id: number): Observable<number> {
        return Observable.of(0);
    }
}
