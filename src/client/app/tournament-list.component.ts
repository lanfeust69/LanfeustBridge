import {Component, Inject} from 'angular2/core';
import {ROUTER_DIRECTIVES, Router} from 'angular2/router';
import {TOURNAMENT_SERVICE, TournamentService} from './tournament.service';

@Component({
    selector: 'tournament-list',
    template: `
<div class="list-group">
  <a [routerLink]="['Tournament', { id: tournamentName.id }]" class="list-group-item" *ngFor="let tournamentName of _tournamentNames">{{tournamentName.name}}</a>
</div>
<button type="button" class='btn btn-block' (click)="createTournament()">Create new tournament</button>
    `,
    directives: [ROUTER_DIRECTIVES]
})
export class TournamentListComponent {
    _tournamentNames: {id: number; name: string}[] = [];

    constructor(
        private _router: Router,
        @Inject(TOURNAMENT_SERVICE) private _tournamentService: TournamentService)
    {}

    ngOnInit() {
        this._tournamentService.getNames().then(names => { this._tournamentNames = names; });
    }

    public createTournament() {
        this._router.navigate( ['NewTournament', { id: "-1" }] );
    }
}
