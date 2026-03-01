import { InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';

export const USER_SERVICE = new InjectionToken('UserService');

export class User {
    name = '';
    email = '';
    roles = [];
}

export interface UserService {
    currentUser: string;
    isCurrentUserAdmin: boolean;

    isLoggedIn(): Observable<boolean>;

    getAllUsers(): Observable<User[]>;
}
