import {Hand} from './hand'

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
    scores: [
        {
            contract: string;
        }
    ];
    constructor(id: number) {
        this.id = id;
        this.dealer = ["N", "E", "S", "W"][(id - 1) % 4];
        switch ((id - 1) % 16) {
            case 0: case 7: case 10: case 13:
                this.vulnerability = "None";
                break;
            case 1: case 4: case 11: case 14:
                this.vulnerability = "NS";
                break;
            case 2: case 5: case 8: case 15:
                this.vulnerability = "EW";
                break;
            case 3: case 6: case 9: case 12:
                this.vulnerability = "Both";
                break;
        }
        this.hands = {
            west: new Hand(),
            north: new Hand(),
            east: new Hand(),
            south: new Hand()
        }
    }
}