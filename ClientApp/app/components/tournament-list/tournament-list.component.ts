import {Component, Inject} from '@angular/core';
import {Router} from '@angular/router';
import {TOURNAMENT_SERVICE, TournamentService} from '../../services/tournament/tournament.service';

@Component({
    selector: 'tournament-list',
    template: `
<h1 class="text-center">Lanfeust Bridge</h1>
<div class="list-group">
  <a [routerLink]="['tournament', tournamentName.id]" class="list-group-item" *ngFor="let tournamentName of _tournamentNames">{{tournamentName.name}}</a>
</div>
<button type="button" class='btn btn-block' (click)="createTournament()">Create new tournament</button>
    `
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
        this._router.navigate( ['new-tournament', -1 ] );
    }
}
