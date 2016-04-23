import {Deal} from './deal';

export enum Status {
    Setup,
    Running,
    Finished
}

export class Tournament {
    name: string;
    date: Date = new Date;
    movement: string = "Mitchell";
    players: [
        {
            name: string;
            score: number;
            rank: number;
        }
    ]; // names, index in array is id
    nbRound: number;
    nbTables: number;
    deals: Deal[];
    
    status: Status = Status.Setup;
    positions: [[[
        {
            west: number,
            north: number,
            east: number,
            south: number
        }
    ]]]; // indexed by table, round, boardInRound

    constructor(name: string) {
        this.name = name;
    }
}
