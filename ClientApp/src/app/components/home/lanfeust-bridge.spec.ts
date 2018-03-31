import {Component} from '@angular/core';
import {TestBed, inject} from '@angular/core/testing';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {AlertService} from '../../services/alert/alert.service';
import {LanfeustBridgeApp} from './lanfeust-bridge.app';

// tslint:disable-next-line:component-selector
@Component({selector: 'router-outlet', template: ''})
export class RouterOutletStubComponent { }

class RouterStub {
    navigateByUrl(url: string) { return url; }
}

beforeEach(() => TestBed.configureTestingModule({
    imports: [NgbModule.forRoot()],
    declarations: [LanfeustBridgeApp, RouterOutletStubComponent],
    providers: [
        AlertService // simple enough to keep it without a stub
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
