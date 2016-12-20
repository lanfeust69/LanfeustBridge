import {describe, it, expect, beforeEachProviders, inject} from '@angular/core/testing';
import {DealServiceMock} from './deal.service.mock';

beforeEachProviders(() => [DealServiceMock]);

describe('Service Mock: Deal', () => {
    describe('#createRandomDeal', () => {
        it('should return a valid deal', inject([DealServiceMock], (ds: DealServiceMock) => {
            let deal = ds.createRandomDeal(0, 3);
            expect(deal.id).toBe(3);
            expect(deal.dealer).toBe("S");
            expect(deal.vulnerability).toBe("EW");
        }));
    });
});
