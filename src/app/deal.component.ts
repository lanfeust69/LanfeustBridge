import {Component, Input} from 'angular2/core';
import {Deal} from './deal'

@Component({
    templateUrl: 'app/deal.html',
    selector: 'deal',
})
export class DealComponent {
    deal: Deal = new Deal;
}
