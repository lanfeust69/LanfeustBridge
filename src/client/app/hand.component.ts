import {Component, Input} from 'angular2/core';
import {Hand} from './hand';
import {Suit} from './types';
import {SuitComponent} from './suit.component';

@Component({
    selector: 'hand',
    templateUrl: 'app/hand.html',
    directives: [SuitComponent]
})
export class HandComponent {
    Suit = Suit;
    @Input() hand: Hand;
}
