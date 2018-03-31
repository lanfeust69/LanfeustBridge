import { isPlatformBrowser } from '@angular/common';
import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';

import { TOURNAMENT_SERVICE, TournamentService } from '../../services/tournament/tournament.service';

@Component({
    selector: 'lanfeust-bridge-tournament-list',
    templateUrl: './tournament-list.html'
})
export class TournamentListComponent implements OnInit {
    _tournamentNames: {id: number; name: string}[] = [];

    constructor(
        private _router: Router,
        @Inject(PLATFORM_ID) private _platformId: Object,
        @Inject(TOURNAMENT_SERVICE) private _tournamentService: TournamentService) {}

    ngOnInit() {
        this.getTournamentNames();
        if (isPlatformBrowser(this._platformId))
            this._tournamentService.newTournamentObservable.subscribe(this.getTournamentNames.bind(this));
    }

    getTournamentNames() {
        this._tournamentService.getNames().subscribe(names => { this._tournamentNames = names; });
    }

    public createTournament() {
        this._router.navigate(['new-tournament']);
    }
}
