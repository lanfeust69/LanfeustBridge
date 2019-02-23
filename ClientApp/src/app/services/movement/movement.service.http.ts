import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { MovementDescription } from '../../movement';
import { MovementService } from './movement.service';

@Injectable()
export class MovementServiceHttp implements MovementService {
    private _baseUrl: string;

    constructor(private _http: HttpClient, @Inject('BASE_URL') originUrl: string) {
        this._baseUrl = originUrl + 'api/movement';
    }

    getMovements(): Observable<MovementDescription[]> {
        return this._http.get<MovementDescription[]>(this._baseUrl);
    }
}
