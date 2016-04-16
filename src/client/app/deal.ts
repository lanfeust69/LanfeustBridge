import {Hand} from './hand'
export class Deal {
    id: number = 1;
    dealer: string = 'N';
    vulnerability: string = 'None';
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
    constructor() {
        this.hands = {
            west: new Hand(),
            north: new Hand(),
            east: new Hand(),
            south: new Hand()
        }
    }
}