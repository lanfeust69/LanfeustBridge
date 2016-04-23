import {Component, Input} from 'angular2/core';
import {Score} from './score';
import {SuitComponent} from './suit.component';

@Component({
    selector: 'scores',
    templateUrl: 'app/score.html',
    styles: [
        'table { text-align: center }',
        'th { text-align: center }'
    ],
    directives: [SuitComponent]
})
export class ScoreComponent {
    @Input() scores: Score[];
    @Input() individual: boolean = false;
    @Input() matchpoints: boolean = false;
}
