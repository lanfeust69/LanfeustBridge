import { Inject, Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

import { MovementDescription } from '../../movement';
import { MovementService } from './movement.service';

@Injectable()
export class MovementServiceHttp implements MovementService {
    private _baseUrl: string;

    constructor(private _http: Http, @Inject('BASE_URL') originUrl: string) {
        this._baseUrl = originUrl + 'api/movement';
    }

    getMovements(): Observable<MovementDescription[]> {
        return this._http.get(this._baseUrl).map(res => res.json());
    }
}
