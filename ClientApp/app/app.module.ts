import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {UniversalModule} from 'angular2-universal';
import {DealComponent} from './components/deal/deal.component';
import {HandComponent} from './components/hand/hand.component';
import {ScoreComponent} from './components/score/score.component';
import {ScoreFormComponent} from './components/score-form/score-form.component';
import {ScoreSheetComponent} from './components/score-sheet/score-sheet.component';
import {SuitComponent} from './components/suit/suit.component';
import {TournamentComponent} from './components/tournament/tournament.component';
import {TournamentListComponent} from './components/tournament-list/tournament-list.component';
import {LanfeustBridgeApp} from './components/home/lanfeust-bridge.app';
import {TOURNAMENT_SERVICE} from './services/tournament/tournament.service';
import {TournamentServiceHttp} from './services/tournament/tournament.service.http';
// import {TournamentServiceMock} from './services/tournament/tournament.service.mock';
import {DEAL_SERVICE} from './services/deal/deal.service';
import {DealServiceHttp} from './services/deal/deal.service.http';
// import {DealServiceMock} from './services/deal/deal.service.mock';
import {MOVEMENT_SERVICE} from './services/movement/movement.service';
import {MovementServiceHttp} from './services/movement/movement.service.http';
// import {MovementServiceMock} from './services/movement/movement.service.mock';
import {AlertService} from './services/alert/alert.service';

@NgModule({
    bootstrap: [ LanfeustBridgeApp ],
    declarations: [
        LanfeustBridgeApp, DealComponent, HandComponent, ScoreComponent, ScoreFormComponent, ScoreSheetComponent,
        SuitComponent, TournamentComponent, TournamentListComponent
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        FormsModule,
        NgbModule.forRoot(),
        RouterModule.forRoot([
            { path: '', component: TournamentListComponent },
            { path: 'tournament/:id', component: TournamentComponent },
            { path: 'tournament/:tournamentId/deal/:dealId', component: DealComponent },
            { path: 'tournament/:tournamentId/scoresheet/:player', component: ScoreSheetComponent },
            { path: 'new-tournament', component: TournamentComponent },
            { path: '**', redirectTo: '' }
        ])
    ],
    providers: [
        { provide: TOURNAMENT_SERVICE, useClass: TournamentServiceHttp },
        { provide: DEAL_SERVICE, useClass: DealServiceHttp },
        { provide: MOVEMENT_SERVICE, useClass: MovementServiceHttp },
        // { provide: TOURNAMENT_SERVICE, useClass: TournamentServiceMock },
        // { provide: DEAL_SERVICE, useClass: DealServiceMock },
        // { provide: MOVEMENT_SERVICE, useClass: MovementServiceMock },
        { provide: AlertService, useClass: AlertService }
    ]
})
export class AppModule {
}
