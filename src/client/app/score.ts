import {Deal} from './deal';
import {Suit} from './types'

export class Score {
    dealId: number;
    round: number;
    players: {
        north: string;
        south: string;
        east: string;
        west: string;
    }
    contract: {
        declarer: string;
        level: number;
        suit: Suit;
        doubled: boolean;
        redoubled: boolean;
    }
    tricks: number;
    score: number;
    nsResult: number;
    ewResult: number;
}
