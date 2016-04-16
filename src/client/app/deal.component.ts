import {Component, Input} from 'angular2/core';
import {Deal} from './deal';
import {HandComponent} from './hand.component';

@Component({
    selector: 'deal',
    templateUrl: 'app/deal.html',
    directives: [HandComponent]
})
export class DealComponent {
    deal: Deal = new Deal;
}
