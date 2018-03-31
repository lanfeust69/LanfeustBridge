import { Component, Input } from '@angular/core';

import { Hand } from '../../hand';
import { Suit } from '../../types';
import { SuitComponent } from '../suit/suit.component';

@Component({
    selector: 'lanfeust-bridge-hand',
    templateUrl: './hand.html'
})
export class HandComponent {
    Suit = Suit;
    @Input() hand: Hand;
}
