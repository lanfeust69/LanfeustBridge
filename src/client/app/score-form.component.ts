import {EventEmitter, Component, Input, Output} from 'angular2/core';
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
    _score: Score; // bound to score as a property, so that we can update _nbTricksDisplay
    @Output() validated = new EventEmitter();
    
    _suits = [Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades, Suit.NoTrump];
    _nbTricksDisplay = 0;

    ngOnInit() {
        console.log(this.score);
    }

    doubleFixup(doubled: boolean) {
        if (doubled)
            this.score.contract.redoubled = false
    }

    redoubleFixup(redoubled: boolean) {
        if (redoubled)
            this.score.contract.doubled = false
    }

    tricksFixup() {
        let tricks = 6 + this.score.contract.level + this._nbTricksDisplay;
        if (tricks < 0) {
            tricks = 0;
            this._nbTricksDisplay = -(6 + this.score.contract.level);
        }
        if (tricks > 13) {
            tricks = 13;
            this._nbTricksDisplay = 7 - this.score.contract.level;
        }
        this.score.tricks = tricks;
    }

    changeTricks(by: number) {
        this._nbTricksDisplay += by;
        if (this.score.contract.level) {
            this.tricksFixup();
        }
    }

    get score() {
        return this._score;
    }

    @Input('score') set score(value) {
        if (value.contract.level != undefined && value.tricks != undefined)
            this._nbTricksDisplay = value.tricks - 6 - value.contract.level;
        else
            this._nbTricksDisplay = 0;
        this._score = value;
    }

    get tricksDisplay() {
        if (this._nbTricksDisplay == 0)
            return "=";
        if (this._nbTricksDisplay < 0)
            return "" + this._nbTricksDisplay;
        return "+" + this._nbTricksDisplay;
    }

    get computedScore() {
        if (!this.isValid)
            return "";
        let result = this.score.computeScore()
        return (result > 0 ? "+" : "") + result;
    }

    get isValid(): boolean {
        if (this.score.contract.level == 0)
            return true;
        return this.score.contract.level !== undefined && this.score.contract.suit !== undefined
            && this.score.contract.declarer !== undefined;
    }

    onSubmit() {
        console.log("onSubmit");
        this.validated.next(this.score);
    }
}
