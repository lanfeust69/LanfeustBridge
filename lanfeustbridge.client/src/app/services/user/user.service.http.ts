import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { User, UserService } from './user.service';

@Injectable()
export class UserServiceHttp implements UserService {
    private _baseUrl: string;
    private _currentUser = '';
    private _isCurrentUserAdmin = false;

    constructor(private _http: HttpClient, @Inject('BASE_URL') originUrl: string) {
        this._baseUrl = originUrl + 'api/user';
    }

    get currentUser(): string {
        return this._currentUser;
    }

    get isCurrentUserAdmin(): boolean {
        return this._isCurrentUserAdmin;
    }

    isLoggedIn(): Observable<boolean> {
        if (this._currentUser)
            return of(true);
        return this._http.get<User>(`${this._baseUrl}/current`).pipe(
            map(user => {
                this._currentUser = user.name;
                this._isCurrentUserAdmin = user.roles.indexOf('Admin') !== -1;
                return true;
            }),
            catchError(error => {
                console.log('probably not authenticated', error);
                return of(false);
            }));
    }

    getAllUsers(): Observable<User[]> {
        return this._http.get<User[]>(this._baseUrl);
    }
}
