import {Component, Input} from '@angular/core';
import {Suit} from '../../types';

@Component({
    selector: 'suit',
    template: `<span style="color: {{suitColor()}}">&nbsp;<span [innerHTML]="suitSymbol()"></span>&nbsp;</span>`,
})
export class SuitComponent {
    @Input() suit: Suit;

    suitColor() {
        switch (this.suit) {
            case Suit.Clubs: return "darkGreen"
            case Suit.Diamonds: return "orangeRed"
            case Suit.Hearts: return "red"
            case Suit.Spades: return "darkBlue"
            case Suit.NoTrump: return "black"
        }
    }

    suitSymbol() {
        switch (this.suit) {
            case Suit.Clubs: return "&clubs;"
            case Suit.Diamonds: return "&diams;"
            case Suit.Hearts: return "&hearts;"
            case Suit.Spades: return "&spades;"
            case Suit.NoTrump: return "NT"
        }
    }
}
