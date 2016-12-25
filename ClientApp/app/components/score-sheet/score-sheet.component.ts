import {Component, Input, Inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router, Routes, Params} from '@angular/router';
import {Score} from '../../score';
import {Deal} from '../../deal';
import {DealService, DEAL_SERVICE} from '../../services/deal/deal.service';
import {HandComponent} from '../hand/hand.component';
import {ScoreComponent} from '../score/score.component';

@Component({
    selector: 'score-sheet',
    template: `<scores [forPlayers]="true" [scores]="_scores" [tournamentId]="_tournamentId"></scores>`,
})
export class ScoreSheetComponent {
    _scores: Score[] = [];
    _tournamentId: number;
    _player: string;
    
    constructor(
        private _router: Router,
        private _route: ActivatedRoute,
        @Inject(DEAL_SERVICE) private _dealService: DealService) {}
    
    ngOnInit() {
        this._route.params.switchMap((params: Params) => {
            this._tournamentId = +params['tournamentId'];
            this._player = params['player'];
            console.log("tournamentId is " + this._tournamentId + ", player is " + this._player);
            return this._dealService.getDeals(this._tournamentId);
        }).subscribe((deals: Deal[]) => {
            this._scores = deals.reduce<Score[]>((acc, deal) => acc.concat(deal.scores), [])
                .filter(s => s.entered && (s.players.north == this._player || s.players.south == this._player || s.players.east == this._player || s.players.west == this._player));
        });
    }
}
