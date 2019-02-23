import { InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';

import { MovementDescription } from '../../movement';

export const MOVEMENT_SERVICE = new InjectionToken('MovementService');

export interface MovementService {
    getMovements(): Observable<MovementDescription[]>;
}
