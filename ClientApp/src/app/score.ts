import { Deal } from './deal';
import { Suit } from './types';

export class Contract {
    declarer: string;
    level: number;
    suit: Suit;
    doubled: boolean;
    redoubled: boolean;
}

export class Score {
    dealId: number;
    vulnerability: string;
    round: number;
    entered: boolean;
    players: {
        north: string;
        south: string;
        east: string;
        west: string;
    };
    contract: Contract = new Contract;
    tricks: number;
    score: number;
    nsResult: number;
    ewResult: number;

    public static computeScore(s: Score): number {
        if (s.vulnerability === undefined || s.contract === undefined || s.contract.level === undefined
            || s.contract.level === 0 || s.contract.suit === undefined
            || s.contract.declarer === undefined || s.tricks === undefined)
            return 0;
        const level = s.contract.level;
        const doubled: boolean = s.contract.doubled;
        const redoubled: boolean = s.contract.redoubled;
        const vulnerable = s.vulnerability === 'Both'
            || (s.vulnerability !== 'None' && s.vulnerability.indexOf(s.contract.declarer) !== -1);
        const sign = s.contract.declarer === 'N' || s.contract.declarer === 'S' ? 1 : -1;
        const result = s.tricks - 6 - level;
        let score = 0;
        if (result < 0) {
            if (!doubled && !redoubled)
                return sign * result * (vulnerable ? 100 : 50);
            score = result * 300 + 100;
            if (!vulnerable)
                score += result < -3 ? 300 : result * -100;
            if (redoubled)
                score *= 2;
            return sign * score;
        }
        let trickValue = 0;
        switch (s.contract.suit) {
            case Suit.Clubs:
            case Suit.Diamonds:
                trickValue = 20;
                break;
            case Suit.Hearts:
            case Suit.Spades:
                trickValue = 30;
                break;
            case Suit.NoTrump:
                score = 10;
                trickValue = 30;
                break;
        }
        score += level * trickValue;
        score *= redoubled ? 4 : (doubled ? 2 : 1);
        score += score >= 100 ? (vulnerable ? 500 : 300) : 50;
        if (level === 6)
            score += vulnerable ? 750 : 500;
        if (level === 7)
            score += vulnerable ? 1500 : 1000;
        let overtrickValue = trickValue;
        if (doubled || redoubled) {
            score += redoubled ? 100 : 50;
            overtrickValue = vulnerable ? 200 : 100;
            if (redoubled)
                overtrickValue *= 2;
        }
        return sign * (score + result * overtrickValue);
    }
}
