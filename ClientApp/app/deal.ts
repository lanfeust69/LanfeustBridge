import {Hand} from './hand';
import {Score} from './score';

export class Deal {
    id: number;
    dealer: string;
    vulnerability: string;
    hands: {
        west: Hand;
        north: Hand;
        east: Hand;
        south: Hand;
    };
    scores: Score[] = [];

    constructor(id: number) {
        this.id = id;
        this.dealer = Deal.computeDealer(id);
        this.vulnerability = Deal.computeVulnerability(id);
        this.hands = {
            west: new Hand(),
            north: new Hand(),
            east: new Hand(),
            south: new Hand()
        }
    }

    public static computeDealer(id: number) {
        return ["N", "E", "S", "W"][(id - 1) % 4];
    }

    public static computeVulnerability(id: number) {
        switch ((id - 1) % 16) {
            case 0: case 7: case 10: case 13:
                return "None";
            case 1: case 4: case 11: case 14:
                return "NS";
            case 2: case 5: case 8: case 15:
                return "EW";
            case 3: case 6: case 9: case 12:
                return "Both";
        }
    }
}
