import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';

import { Deal } from '../../deal';
import { Score } from '../../score';
import { DealService } from './deal.service';

@Injectable()
export class DealServiceMock implements DealService {
    private _deals: Deal[][] = [];

    getDeal(tournament: number, id: number): Observable<{deal: Deal, hasNext: boolean}> {
        if (!this._deals[tournament])
            this._deals[tournament] = [];
        if (!this._deals[tournament][id - 1])
            this._deals[tournament][id - 1] = this.createRandomDeal(tournament, id);
        return of({deal: this._deals[tournament][id - 1], hasNext: true}).pipe(delay(400)); // 0.4 seconds
    }

    getDeals(tournament: number): Observable<Deal[]> {
        if (!this._deals[tournament])
            return of([]);
        return of(this._deals[tournament]);
    }

    getScore(tournament: number, id: number, round: number, table: number): Observable<Score> {
        if (!this._deals[tournament] || !this._deals[tournament][id - 1] ||
            !this._deals[tournament][id - 1].scores.find(s => s.round === round && s.table === table)) {
            const score = new Score;
            score.dealId = id;
            score.round = round;
            score.table = table;
            score.vulnerability = Deal.computeVulnerability(id);
            return of(score);
        }
        return of(this._deals[tournament][id - 1].scores.find(s => s.round === round && s.table === table));
    }

    postScore(tournament: number, score: Score): Observable<Score> {
        if (!this._deals[tournament])
            this._deals[tournament] = [];
        if (!this._deals[tournament][score.dealId - 1])
            this._deals[tournament][score.dealId - 1] = this.createRandomDeal(tournament, score.dealId);
        score.score = Score.computeScore(score);
        console.log('score of deal ' + score.dealId + ' for round ' + score.round + 'and table ' + score.table + ' received');
        this._deals[tournament][score.dealId - 1].scores.push(score);
        // TODO : update nsResult and ewResult
        return of(score);
    }

    createRandomDeal(tournament: number, id: number): Deal {
        const deal = new Deal(id);
        const cards: number[] = [];
        for (let i = 0; i < 52; i++)
            cards[i] = i;
        for (let i = 52; i > 0; i--) {
            const dest = Math.floor(Math.random() * i);
            const x = cards[i - 1];
            cards[i - 1] = cards[dest];
            cards[dest] = x;
        }
        const suits = ['spades', 'hearts', 'diamonds', 'clubs'];
        const cardNames = ['A', 'K', 'Q', 'J', '10', '9', '8', '7', '6', '5', '4', '3', '2'];
        for (let i = 0; i < 4; i++) {
            const player = ['west', 'north', 'east', 'south'][i];
            const hand = cards.slice(i * 13, (i + 1) * 13).sort(function (a, b) { return a - b; });
            for (let j = 0; j < 13; j++)
                deal.hands[player][suits[Math.floor(hand[j] / 13)]].push(cardNames[hand[j] % 13]);
        }
        return deal;
    }
}
