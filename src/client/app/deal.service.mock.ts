import {Injectable} from 'angular2/core';
import {Suit} from './types';
import {Deal} from './deal';
import {Score} from './score';
import {DealService} from './deal.service';

@Injectable()
export class DealServiceMock implements DealService {
    private _deals: Deal[][] = [];

    getDeal(tournament: number, id: number) : Promise<Deal> {
        if (!this._deals[tournament])
            this._deals[tournament] = [];
        if (!this._deals[tournament][id - 1])
            this._deals[tournament][id - 1] = this.createRandomDeal(tournament, id);
        return new Promise<Deal>(resolve => setTimeout(() => resolve(this._deals[tournament][id - 1]), 400)); // 0.4 seconds
    }

    getScore(tournament: number, id: number, round: number) : Promise<Score> {
        if (!this._deals[tournament] || !this._deals[tournament][id - 1] || !this._deals[tournament][id - 1].scores[round]) {
            let score = new Score;
            score.dealId = id;
            score.round = round;
            return Promise.resolve(score);
        }
        return Promise.resolve(this._deals[tournament][id - 1].scores[round]);
    }

    postScore(tournament: number, score: Score) : Promise<Score> {
        // if (id < 0 || id >= this._deals.length || !this._deals[id]) 
        //     return Promise.reject<Score>("No tournament with id '" + id + "' found");
        if (!this._deals[tournament])
            this._deals[tournament] = [];
        if (!this._deals[tournament][score.dealId - 1])
            this._deals[tournament][score.dealId - 1] = this.createRandomDeal(tournament, score.dealId);
        score.score = score.computeScore();
        this._deals[tournament][score.dealId - 1].scores[score.round] = score;
        // TODO : update nsResult and ewResult
        return Promise.resolve(score);
    }

    createRandomDeal(tournament: number, id: number) : Deal {
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
        let suits = ["spades", "hearts", "diamonds", "clubs"];
        let cardNames = ["A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2"];
        for (let i = 0; i < 4; i++) {
            let player = ["west", "north", "east", "south"][i];
            let hand = cards.slice(i * 13, (i + 1) * 13).sort(function(a, b) { return a - b });
            for (let j = 0; j < 13; j++)
                deal.hands[player][suits[Math.floor(hand[j] / 13)]].push(cardNames[hand[j] % 13]);
        }

        return deal;
    }
}
