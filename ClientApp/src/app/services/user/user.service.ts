import { InjectionToken } from '@angular/core';
import { Observable } from 'rxjs/Observable';

export const USER_SERVICE = new InjectionToken('UserService');

export class User {
    name = '';
    email = '';
    role = '';
}

export interface UserService {
    currentUser: string;

    isLoggedIn(): Observable<boolean>;

    getAllUsers(): Observable<User[]>;
}
