import {Component, Input} from 'angular2/core';
import {ROUTER_DIRECTIVES} from 'angular2/router';
import {Score} from './score';
import {SuitComponent} from './suit.component';

@Component({
    selector: 'scores',
    templateUrl: 'app/score.html',
    styles: [
        'table { text-align: center }',
        'th { text-align: center }'
    ],
    directives: [ROUTER_DIRECTIVES, SuitComponent]
})
export class ScoreComponent {
    @Input() scores: Score[];
    @Input() tournamentId: number;
    @Input() forPlayers:boolean = false;
    @Input() individual: boolean = false;
    @Input() matchpoints: boolean = false;

    get filteredScores() {
        return this.scores.filter(s => s.entered);
    }
}
