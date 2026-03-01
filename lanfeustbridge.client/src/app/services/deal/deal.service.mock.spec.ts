import { TestBed, inject } from '@angular/core/testing';
import { DealServiceMock } from './deal.service.mock';

beforeEach(() => TestBed.configureTestingModule({
    providers: [DealServiceMock]})
);

describe('Service Mock: Deal', () => {
    describe('#createRandomDeal', () => {
        it('should return a valid deal', inject([DealServiceMock], (ds: DealServiceMock) => {
            const deal = ds.createRandomDeal(0, 3);
            expect(deal.id).toBe(3);
            expect(deal.dealer).toBe('S');
            expect(deal.vulnerability).toBe('EW');
        }));
    });
});
