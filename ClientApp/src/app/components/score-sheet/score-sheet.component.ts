import { Component, Input, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Routes, Params } from '@angular/router';

import { Score } from '../../score';
import { Deal } from '../../deal';
import { DealService, DEAL_SERVICE } from '../../services/deal/deal.service';
import { HandComponent } from '../hand/hand.component';
import { ScoreComponent } from '../score/score.component';

@Component({
    selector: 'lanfeust-bridge-score-sheet',
    template: `<lanfeust-bridge-scores [forPlayers]="true" [scores]="_scores" [tournamentId]="_tournamentId" [individual]="_individual">
    </lanfeust-bridge-scores>`,
})
export class ScoreSheetComponent implements OnInit {
    _scores: Score[] = [];
    _tournamentId: number;
    _player: string;
    _individual = false;

    constructor(
        private _router: Router,
        private _route: ActivatedRoute,
        @Inject(DEAL_SERVICE) private _dealService: DealService) {}

    ngOnInit() {
        this._route.queryParams.subscribe(q => this._individual = 'individual' in q);
        this._route.params.switchMap((params: Params) => {
            this._tournamentId = +params['tournamentId'];
            this._player = params['player'];
            console.log('tournamentId is ' + this._tournamentId + ', player is ' + this._player);
            return this._dealService.getDeals(this._tournamentId);
        }).subscribe((deals: Deal[]) => {
            this._scores = deals.reduce<Score[]>((acc, deal) => acc.concat(deal.scores), [])
                .filter(s => s.entered &&
                    (s.players.north === this._player || s.players.south === this._player ||
                     s.players.east === this._player || s.players.west === this._player));
        });
    }
}
