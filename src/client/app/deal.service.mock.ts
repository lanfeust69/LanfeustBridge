import {Injectable} from 'angular2/core';
import {Deal} from './deal';
import {DealService} from './deal.service';

@Injectable()
export class DealServiceMock implements DealService {
    getDeal(tournament: string, id: number) : Deal {
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
