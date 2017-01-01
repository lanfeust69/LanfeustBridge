import {OpaqueToken} from '@angular/core';
import {Observable} from 'rxjs/Rx';
import {MovementDescription} from '../../movement';

export let MOVEMENT_SERVICE = new OpaqueToken('MovementService');

export interface MovementService {
    getMovements(): Observable<MovementDescription[]>;
}
