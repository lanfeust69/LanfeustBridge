import {describe, it, expect, beforeEachProviders, inject} from 'angular2/testing';
import {LanfeustBridgeApp} from './lanfeust-bridge.app';

beforeEachProviders(() => [LanfeustBridgeApp]);

describe('App: LanfeustBridge', () => {
    describe('#randomDealId', () => {
        it('should be an integer between 1 and 32', inject([LanfeustBridgeApp], (app: LanfeustBridgeApp) => {
            // check if a number is an integer is really ugly 
            expect(app.randomDeal).toBe(parseInt(app.randomDeal.toString()));
            expect(app.randomDeal).toBeGreaterThan(0);
            expect(app.randomDeal).toBeLessThan(33);
        }));
    });
});
