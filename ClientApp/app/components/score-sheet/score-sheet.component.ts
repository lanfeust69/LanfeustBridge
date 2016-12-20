import {Component, Input, Inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router, Routes} from '@angular/router';
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
    
    constructor(
        private _router: Router,
        private _route: ActivatedRoute,
        @Inject(DEAL_SERVICE) private _dealService: DealService) {}
    
    ngOnInit() {
        this._tournamentId = +this._route.snapshot.params['tournamentId'];
        let player = this._route.snapshot.params['player'];
        console.log("tournamentId is " + this._tournamentId + ", player is " + player);
        this._dealService.getDeals(this._tournamentId).then(deals  => {
            this._scores = (<Deal[]>deals).reduce<Score[]>((acc, deal) => acc.concat(deal.scores), [])
                .filter(s => s.entered && (s.players.north == player || s.players.south == player || s.players.east == player || s.players.west == player));
        });
    }
}
