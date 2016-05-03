import {Deal} from './deal';
import {Suit} from './types'

export class Contract {
    declarer: string;
    level: number;
    suit: Suit;
    doubled: boolean;
    redoubled: boolean;    
}

export class Score {
    dealId: number;
    round: number;
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

    computeScore(): number {
        if (this.dealId === undefined || this.contract === undefined || this.contract.level === undefined
            || this.contract.level === 0 || this.contract.suit === undefined
            || this.contract.declarer === undefined || this.tricks == undefined)
            return 0;
        let level = this.contract.level;
        let doubled: boolean = this.contract.doubled;
        let redoubled: boolean = this.contract.redoubled;
        let vulnerability = Deal.computeVulnerability(this.dealId);
        let vulnerable = vulnerability == "Both" || (vulnerability != "None" && vulnerability.indexOf(this.contract.declarer) != -1);
        let sign = this.contract.declarer == "N" || this.contract.declarer == "S" ? 1 : -1;
        let result = this.tricks - 6 - level;
        let score = 0;
        if (result < 0) {
            if (!doubled && !redoubled)
                return sign * result * (vulnerable ? 100: 50);
            score = result * 300 + 100;
            if (!vulnerable)
                score += result < -3 ? 300 : result * -100;
            if (redoubled)
                score *= 2;
            return sign * score;
        }
        let trickValue = 0;
        switch (this.contract.suit) {
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
        if (level == 6)
            score += vulnerable ? 1000 : 500;
        if (level == 7)
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
