import {Component, Input} from 'angular2/core';
import {Hand} from './hand'

@Component({
    templateUrl: 'app/hand.html',
    selector: 'hand',
})
export class HandComponent {
    @Input() hand: Hand;
}
