import {Component, Inject} from '@angular/core';
import {Router} from '@angular/router';
import {TOURNAMENT_SERVICE, TournamentService} from '../../services/tournament/tournament.service';

@Component({
    selector: 'tournament-list',
    templateUrl: './tournament-list.html'
})
export class TournamentListComponent {
    _tournamentNames: {id: number; name: string}[] = [];

    constructor(
        private _router: Router,
        @Inject(TOURNAMENT_SERVICE) private _tournamentService: TournamentService)
    {}

    ngOnInit() {
        this._tournamentService.getNames().subscribe(names => { this._tournamentNames = names; });
    }

    public createTournament() {
        this._router.navigate(['new-tournament']);
    }
}
