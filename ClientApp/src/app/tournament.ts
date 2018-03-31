export enum Status {
    Setup,
    Running,
    Finished
}

export class Player {
    name: string = '';
    score: number = 0;
    rank: number = 0;
}

export class Position {
    table: number;
    deals: number[];
    west: number;
    north: number;
    east: number;
    south: number;
}

export class Tournament {
    id: number = 0;
    name: string;
    date: Date = new Date;
    movement: string = '';
    scoring: string = '';
    nbTables: number;
    nbRounds: number;
    nbDealsPerRound: number;
    nbDeals: number;
    players: Player[] = []; // names, index in array is id

    status: Status = Status.Setup;
    currentRound: number;
    positions: Position[][]; // indexed by round, player
}
