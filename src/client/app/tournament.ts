import {Deal} from './deal';

export enum Status {
    Setup,
    Running,
    Finished
}

export class Tournament {
    id: number = -1;
    name: string;
    date: Date = new Date;
    movement: string = "";
    nbTables: number;
    nbRounds: number;
    players: [
        {
            name: string;
            score: number;
            rank: number;
        }
    ]; // names, index in array is id
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
}
