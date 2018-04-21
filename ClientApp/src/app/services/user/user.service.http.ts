import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { User, UserService } from './user.service';

@Injectable()
export class UserServiceHttp implements UserService {
    private _baseUrl: string;
    private _currentUser = '';

    constructor(private _http: HttpClient, @Inject('BASE_URL') originUrl: string) {
        this._baseUrl = originUrl + 'api/user';
    }

    get currentUser(): string {
        return this._currentUser;
    }

    isLoggedIn(): Observable<boolean> {
        if (this._currentUser)
            return Observable.of(true);
        return this._http.get<User>(`${this._baseUrl}/current`)
            .map(user => {
                this._currentUser = user.name;
                return true;
            })
            .catch(error => {
                console.log('probably not authenticated', error);
                return Observable.of(false);
            });
    }

    getAllUsers(): Observable<User[]> {
        return this._http.get<User[]>(this._baseUrl);
    }
}
