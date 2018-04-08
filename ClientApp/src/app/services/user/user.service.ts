import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

export class User {
    name = '';
    email = '';
    role = '';
}

@Injectable()
export class UserService {
    private _baseUrl: string;
    private _currentUser = '';

    constructor(private _http: HttpClient, @Inject('BASE_URL') originUrl: string) {
        this._baseUrl = originUrl + 'api/tournament/';
    }

    get currentUser(): string {
        return this._currentUser;
    }

    isLoggedIn(): Observable<boolean> {
        if (this._currentUser)
            return Observable.of(true);
        return this._http.get('/api/user', { observe: 'response', responseType: 'text' })
            .map(r => {
                this._currentUser = r.body.toString();
                return true;
            })
            .catch(error => {
                console.log('probably not authenticated', error);
                return Observable.of(false);
            });
    }
}
