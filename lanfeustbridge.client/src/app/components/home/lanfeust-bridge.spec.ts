import { Component } from '@angular/core';
import { TestBed, inject } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AlertService } from '../../services/alert/alert.service';
import { USER_SERVICE } from '../../services/user/user.service';
import { UserServiceMock } from '../../services/user/user.service.mock';

import { LanfeustBridgeApp } from './lanfeust-bridge.app';

beforeEach(() => TestBed.configureTestingModule({
    imports: [NgbModule, RouterTestingModule],
    declarations: [LanfeustBridgeApp],
    providers: [
        AlertService, // simple enough to keep it without a stub
        { provide: USER_SERVICE, useClass: UserServiceMock}
    ]
}));

describe('App: LanfeustBridge', () => {
    describe('#randomDealId', () => {
        it('should be an integer between 1 and 32', () => {
            const app = TestBed.createComponent(LanfeustBridgeApp).componentInstance;
            // check if a number is an integer is really ugly
            expect(app.randomDeal).toBe(parseInt(app.randomDeal.toString()));
            expect(app.randomDeal).toBeGreaterThan(0);
            expect(app.randomDeal).toBeLessThan(33);
        });
    });
});
