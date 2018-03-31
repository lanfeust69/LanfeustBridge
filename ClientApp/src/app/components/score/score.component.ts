import { Component, Input } from '@angular/core';

import { Score } from '../../score';
import { SuitComponent } from '../suit/suit.component';

@Component({
    selector: 'lanfeust-bridge-scores',
    templateUrl: './score.html',
    styles: [
        'table { text-align: center }',
        'th { text-align: center }'
    ]
})
export class ScoreComponent {
    @Input() scores: Score[];
    @Input() tournamentId: number;
    @Input() forPlayers = false;
    @Input() individual = false;
    @Input() matchpoints = false;

    get filteredScores() {
        return this.scores.filter(s => s.entered);
    }
}
