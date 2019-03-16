import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { User, UserService } from './user.service';

@Injectable()
export class UserServiceMock implements UserService {
    currentUser = 'John';
    isCurrentUserAdmin = false;

    isLoggedIn(): Observable<boolean> {
        return of(true);
    }

    getAllUsers(): Observable<User[]> {
        return of([
            { name: 'Alice', email: 'alice@lanfeustbridge.com', roles: [] },
            { name: 'Bob', email: 'bob@lanfeustbridge.com', roles: [] },
            { name: 'Carol', email: 'carol@lanfeustbridge.com', roles: [] },
            { name: 'David', email: 'david@lanfeustbridge.com', roles: [] },
            { name: 'Ethan', email: 'ethan@lanfeustbridge.com', roles: [] },
            { name: 'Fiona', email: 'fiona@lanfeustbridge.com', roles: [] },
            { name: 'George', email: 'george@lanfeustbridge.com', roles: [] },
            { name: 'Harry', email: 'harry@lanfeustbridge.com', roles: [] },
            { name: 'Irving', email: 'irving@lanfeustbridge.com', roles: [] },
            { name: 'John', email: 'john@lanfeustbridge.com', roles: [] },
            { name: 'Kenny', email: 'kenny@lanfeustbridge.com', roles: [] },
            { name: 'Liz', email: 'liz@lanfeustbridge.com', roles: [] },
            { name: 'Mary', email: 'mary@lanfeustbridge.com', roles: [] }
        ]);
    }
}
