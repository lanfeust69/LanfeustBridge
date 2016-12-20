import {Component, Input} from '@angular/core';
import {Score} from '../../score';
import {SuitComponent} from '../suit/suit.component';

@Component({
    selector: 'scores',
    templateUrl: './score.html',
    styles: [
        'table { text-align: center }',
        'th { text-align: center }'
    ]
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
