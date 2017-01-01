import {Injectable} from '@angular/core';
import {Http} from '@angular/http';
import {Observable} from 'rxjs/Rx';
import {MovementDescription} from '../../movement';
import {MovementService} from './movement.service';

@Injectable()
export class MovementServiceHttp implements MovementService {
    private _baseUrl = 'api/movement';

    constructor(private _http: Http) {}

    getMovements(): Observable<MovementDescription[]> {
        return this._http.get(this._baseUrl).map(res => res.json());
    }
}
