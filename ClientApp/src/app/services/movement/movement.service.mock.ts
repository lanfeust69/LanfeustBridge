import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { MovementDescription } from '../../movement';
import { MovementService } from './movement.service';

@Injectable()
export class MovementServiceMock implements MovementService {
    getMovements(): Observable<MovementDescription[]> {
        return Observable.of([
            {
                id: 'mitchell',
                name: 'Mitchell',
                description: 'Standard Mitchell : NS fixed, EW move to next table, boards to previous',
                nbPlayers: -1,
                minTables: 3,
                maxTables: -1,
                minRounds: 2,
                maxRounds: -1
            },
            {
                id: 'teams',
                name: 'Teams match',
                description: 'Pairs are fixed for 2 rounds, where deals are switched, then pairs switch for next set of deals',
                nbPlayers: -1,
                minTables: 2,
                maxTables: 2,
                minRounds: 2,
                maxRounds: -1
            },
            {
                id: 'triplicate',
                name: 'Triplicate for 6 pairs',
                description: 'Only accepts 15 rounds : 3 rounds playing against each of the other pairs',
                nbPlayers: -1,
                minTables: 3,
                maxTables: 3,
                minRounds: 15,
                maxRounds: 15
            },
            {
                id: 'individual',
                name: 'Individual for 12 players',
                description: 'Only accepts 33 rounds : 3 rounds playing with each of the 11 other players',
                nbPlayers: 12,
                minTables: 3,
                maxTables: 3,
                minRounds: 33,
                maxRounds: 33
            }
        ]);
    }
}
