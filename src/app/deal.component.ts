import {Component, Input} from 'angular2/core';
import {Deal} from './deal';
import {HandComponent} from './hand.component';

@Component({
    templateUrl: 'app/deal.html',
    selector: 'deal',
    directives: [HandComponent]
})
export class DealComponent {
    deal: Deal = new Deal;
}
