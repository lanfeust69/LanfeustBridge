import {Component, Input, Inject} from 'angular2/core';
import {Tournament} from './tournament';
import {TournamentService, TOURNAMENT_SERVICE} from './tournament.service';
import {Deal} from './deal';
import {DealService, DEAL_SERVICE} from './deal.service';
import {DealComponent} from './deal.component';

@Component({
    selector: 'tournament',
    templateUrl: 'app/tournament.html',
    directives: [DealComponent]
})
export class TournamentComponent {
    @Input() name: string;
    @Input() edit: boolean = false;
    _tournament: Tournament;
    
    constructor(
        @Inject(TOURNAMENT_SERVICE) private _tournamentService: TournamentService,
        @Inject(DEAL_SERVICE) private _dealService: DealService) {
            console.log("TournamentComponent constructor");
    }
    
    ngOnInit() {
        this._tournamentService.get(this.name)
            .then(tournament => {
                console.log("tournament service returned", tournament);
                this._tournament = tournament;
            })
            .catch(reason => {
                console.log("no tournament found, create a new one");
                this._tournament = new Tournament(this.name);
                this.edit = true;
            });
    }
}
