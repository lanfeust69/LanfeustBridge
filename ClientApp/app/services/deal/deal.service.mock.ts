import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Rx';
import {Deal} from '../../deal';
import {Score} from '../../score';
import {DealService} from './deal.service';

@Injectable()
export class DealServiceMock implements DealService {
    private _deals: Deal[][] = [];

    getDeal(tournament: number, id: number): Observable<Deal> {
        if (!this._deals[tournament])
            this._deals[tournament] = [];
        if (!this._deals[tournament][id - 1])
            this._deals[tournament][id - 1] = this.createRandomDeal(tournament, id);
        return Observable.of(this._deals[tournament][id - 1]).delay(400); // 0.4 seconds
    }

    getDeals(tournament: number): Observable<Deal[]> {
        if (!this._deals[tournament])
            return Observable.of([]);
        return Observable.of(this._deals[tournament]);
    }

    getScore(tournament: number, id: number, round: number): Observable<Score> {
        if (!this._deals[tournament] || !this._deals[tournament][id - 1] || !this._deals[tournament][id - 1].scores[round]) {
            let score = new Score;
            score.dealId = id;
            score.round = round;
            score.vulnerability = Deal.computeVulnerability(id);
            return Observable.of(score);
        }
        return Observable.of(this._deals[tournament][id - 1].scores[round]);
    }

    postScore(tournament: number, score: Score): Observable<Score> {
        if (!this._deals[tournament])
            this._deals[tournament] = [];
        if (!this._deals[tournament][score.dealId - 1])
            this._deals[tournament][score.dealId - 1] = this.createRandomDeal(tournament, score.dealId);
        score.score = Score.computeScore(score);
        console.log('score of deal ' + score.dealId + ' for round ' + score.round + ' received');
        this._deals[tournament][score.dealId - 1].scores[score.round] = score;
        // TODO : update nsResult and ewResult
        return Observable.of(score);
    }

    createRandomDeal(tournament: number, id: number): Deal {
        let deal = new Deal(id);
        let cards: number[] = [];
        for (let i = 0; i < 52; i++)
            cards[i] = i;
        for (let i = 52; i > 0; i--) {
            let dest = Math.floor(Math.random() * i);
            let x = cards[i - 1];
            cards[i - 1] = cards[dest];
            cards[dest] = x;
        }
        let suits = ['spades', 'hearts', 'diamonds', 'clubs'];
        let cardNames = ['A', 'K', 'Q', 'J', '10', '9', '8', '7', '6', '5', '4', '3', '2'];
        for (let i = 0; i < 4; i++) {
            let player = ['west', 'north', 'east', 'south'][i];
            let hand = cards.slice(i * 13, (i + 1) * 13).sort(function (a, b) { return a - b; });
            for (let j = 0; j < 13; j++)
                deal.hands[player][suits[Math.floor(hand[j] / 13)]].push(cardNames[hand[j] % 13]);
        }
        return deal;
    }
}
