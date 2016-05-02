import {Deal} from './deal';

export enum Status {
    Setup,
    Running,
    Finished
}

export class Player {
    name: string = "";
    score: number = 0;
    rank: number = 0;
}

export class Tournament {
    id: number = -1;
    name: string;
    date: Date = new Date;
    movement: string = "";
    nbTables: number;
    nbRounds: number;
    nbDealsPerRound: number;
    players: Player[] = []; // names, index in array is id
    
    status: Status = Status.Setup;
    positions:
        {
            table: number,
            west: number,
            north: number,
            east: number,
            south: number
        }[][][]; // indexed by player, round
}
