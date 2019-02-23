import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { User, UserService } from './user.service';

@Injectable()
export class UserServiceMock implements UserService {
    currentUser = 'John';

    isLoggedIn(): Observable<boolean> {
        return of(true);
    }

    getAllUsers(): Observable<User[]> {
        return of([
            { name: 'Alice', email: 'alice@lanfeustbridge.com', role: '' },
            { name: 'Bob', email: 'bob@lanfeustbridge.com', role: '' },
            { name: 'Carol', email: 'carol@lanfeustbridge.com', role: '' },
            { name: 'David', email: 'david@lanfeustbridge.com', role: '' },
            { name: 'Ethan', email: 'ethan@lanfeustbridge.com', role: '' },
            { name: 'Fiona', email: 'fiona@lanfeustbridge.com', role: '' },
            { name: 'George', email: 'george@lanfeustbridge.com', role: '' },
            { name: 'Harry', email: 'harry@lanfeustbridge.com', role: '' },
            { name: 'Irving', email: 'irving@lanfeustbridge.com', role: '' },
            { name: 'John', email: 'john@lanfeustbridge.com', role: '' },
            { name: 'Kenny', email: 'kenny@lanfeustbridge.com', role: '' },
            { name: 'Liz', email: 'liz@lanfeustbridge.com', role: '' },
            { name: 'Mary', email: 'mary@lanfeustbridge.com', role: '' }
        ]);
    }
}
