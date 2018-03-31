import { AfterViewInit, Component, Input, Inject, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router, Routes, Params } from '@angular/router';

import { Deal } from '../../deal';
import { DealService, DEAL_SERVICE } from '../../services/deal/deal.service';
import { HandComponent } from '../hand/hand.component';
import { ScoreComponent } from '../score/score.component';

@Component({
    selector: 'lanfeust-bridge-deal',
    templateUrl: './deal.html',
    styles: ['canvas { background-color: limeGreen; /*margin: 10px*/ }',
        '.no-gutter [class*=\'col-\'] { padding-right:0; padding-left:0; vertical-align: middle;}',
    ]
})
export class DealComponent implements AfterViewInit, OnInit {
    @Input() tournamentId: number;
    @Input() id: number;
    deal: Deal;
    @ViewChild('table') tableCanvas;
    viewInitialized = false;

    constructor(
        private _router: Router,
        private _route: ActivatedRoute,
        @Inject(DEAL_SERVICE) private _dealService: DealService) {}

    ngOnInit() {
        this._route.params.switchMap((params: Params) => {
            this.tournamentId = +params['tournamentId'];
            this.id = +params['dealId'];
            console.log('tournamentId is ' + this.tournamentId + ', dealId is ' + this.id);
            return this._dealService.getDeal(this.tournamentId, this.id);
        }).subscribe((deal: Deal) => {
            console.log('deal service returned', deal);
            this.deal = deal;
            if (this.viewInitialized)
                this.drawTable();
        });
    }

    ngAfterViewInit() {
        this.viewInitialized = true;
        if (this.deal)
            this.drawTable();
    }

    drawTable() {
        const context = this.tableCanvas.nativeElement.getContext('2d');
        context.clearRect(0, 0, this.tableCanvas.nativeElement.width, this.tableCanvas.nativeElement.height);
        const size = this.tableCanvas.nativeElement.height;
        const font = (size / 6) + 'px arial';
        context.font = font;
        context.textBaseline = 'middle';
        context.textAlign = 'center';
        context.fillText('#' + this.deal.id, size / 2, size / 2);
        for (let i = 0; i < 4; i++) {
            const x = [0, size / 3, size * 4 / 5, size / 3][i];
            const y = [size / 3, 0, size / 3, size * 4 / 5][i];
            const width = [size / 5, size / 3][i % 2];
            const height = [size / 3, size / 5][i % 2];
            const color = {
                None: ['white', 'white'],
                Both: ['red', 'red'],
                NS: ['white', 'red'],
                EW: ['red', 'white']
            }[this.deal.vulnerability][i % 2];
            context.fillStyle = color;
            context.fillRect(x, y, width, height);
            context.strokeStyle = 'black';
            context.strokeRect(x, y, width, height);
            let name = ['W', 'N', 'E', 'S'][i];
            const textX = [size / 10, size / 2, size * 9 / 10, size / 2][i];
            const textY = [size / 2, size / 10, size / 2, size * 9 / 10][i];
            context.fillStyle = 'black';
            if (name === this.deal.dealer) {
                name = 'D';
                context.font = 'bold ' + font;
            } else {
                context.font = font;
            }
            context.fillText(name, textX, textY);
        }
    }
}
