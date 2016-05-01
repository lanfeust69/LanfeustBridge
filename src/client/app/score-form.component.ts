import {EventEmitter, Component, Input} from 'angular2/core';
import {BUTTON_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {Score} from './score';
import {Suit} from './types';
import {SuitComponent} from './suit.component';

@Component({
    selector: 'score-form',
    templateUrl: 'app/score-form.html',
    directives: [BUTTON_DIRECTIVES, SuitComponent]
})
export class ScoreFormComponent {
    @Input() score: Score;
    validated: EventEmitter<{}> = new EventEmitter();
    
    _levels = [1, 2, 3, 4, 5, 6, 7];
    _suits = [Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades, Suit.NoTrump];

    ngOnInit() {
        console.log(this.score);
    }

    onSubmit() {
        console.log("onSubmit");
        this.validated.next(this.score);
    }
}
