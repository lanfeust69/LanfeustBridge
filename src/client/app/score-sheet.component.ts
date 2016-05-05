import {Component, Input, Inject, OnInit} from 'angular2/core';
import {RouteParams, Router, ROUTER_DIRECTIVES} from 'angular2/router';
import {Score} from './score';
import {Deal} from './deal';
import {DealService, DEAL_SERVICE} from './deal.service';
import {HandComponent} from './hand.component';
import {ScoreComponent} from './score.component';

@Component({
    selector: 'score-sheet',
    template: `<scores [forPlayers]="true" [scores]="_scores" [tournamentId]="_tournamentId"></scores>`,
    directives: [ROUTER_DIRECTIVES, HandComponent, ScoreComponent]
})
export class ScoreSheetComponent {
    _scores: Score[] = [];
    _tournamentId: number;
    
    constructor(
        private _router: Router,
        private _routeParams: RouteParams,
        @Inject(DEAL_SERVICE) private _dealService: DealService) {}
    
    ngOnInit() {
        this._tournamentId = +this._routeParams.get('tournamentId');
        let player = this._routeParams.get('player');
        console.log("tournamentId is " + this._tournamentId + ", player is " + player);
        this._dealService.getDeals(this._tournamentId).then(deals  => {
            this._scores = (<Deal[]>deals).reduce<Score[]>((acc, deal) => acc.concat(deal.scores), [])
                .filter(s => s.entered && (s.players.north == player || s.players.south == player || s.players.east == player || s.players.west == player));
        });
    }
}
