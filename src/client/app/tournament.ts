import {Deal} from './deal';

export class Tournament {
    name: string;
    date: Date;
    movement: string;
    players: [
        {
            name: string;
            score: number;
            rank: number;
        }
    ]; // names, index in array is id
    nbRound: number;
    deals: Deal[];
    
    constructor(name: string) {
        this.name = name;
    }
}
