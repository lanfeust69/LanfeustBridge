import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { DealComponent } from './components/deal/deal.component';
import { HandComponent } from './components/hand/hand.component';
import { ScoreComponent } from './components/score/score.component';
import { ScoreFormComponent } from './components/score-form/score-form.component';
import { ScoreSheetComponent } from './components/score-sheet/score-sheet.component';
import { SuitComponent } from './components/suit/suit.component';
import { TournamentComponent } from './components/tournament/tournament.component';
import { TournamentListComponent } from './components/tournament-list/tournament-list.component';
import { LanfeustBridgeApp } from './components/home/lanfeust-bridge.app';

import { DateInterceptor } from './services/date.interceptor';
import { TOURNAMENT_SERVICE } from './services/tournament/tournament.service';
import { TournamentServiceHttp } from './services/tournament/tournament.service.http';
// import { TournamentServiceMock } from './services/tournament/tournament.service.mock';
import { DEAL_SERVICE } from './services/deal/deal.service';
import { DealServiceHttp } from './services/deal/deal.service.http';
// import { DealServiceMock } from './services/deal/deal.service.mock';
import { MOVEMENT_SERVICE } from './services/movement/movement.service';
import { MovementServiceHttp } from './services/movement/movement.service.http';
// import { MovementServiceMock } from './services/movement/movement.service.mock';
import { USER_SERVICE, UserService } from './services/user/user.service';
import { UserServiceHttp } from './services/user/user.service.http';
import { UserServiceMock } from './services/user/user.service.mock';
import { AlertService } from './services/alert/alert.service';

// import used rxjs bits
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/observable/fromEventPattern';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/merge';
import 'rxjs/add/operator/switchMap';

@NgModule({
    bootstrap: [ LanfeustBridgeApp ],
    declarations: [
        LanfeustBridgeApp, DealComponent, HandComponent, ScoreComponent, ScoreFormComponent, ScoreSheetComponent,
        SuitComponent, TournamentComponent, TournamentListComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        NgbModule.forRoot(),
        FormsModule,
        HttpClientModule,
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
        { provide: HTTP_INTERCEPTORS, useClass: DateInterceptor, multi: true },
        { provide: TOURNAMENT_SERVICE, useClass: TournamentServiceHttp },
        { provide: DEAL_SERVICE, useClass: DealServiceHttp },
        { provide: MOVEMENT_SERVICE, useClass: MovementServiceHttp },
        { provide: USER_SERVICE, useClass: UserServiceHttp },
        // { provide: TOURNAMENT_SERVICE, useClass: TournamentServiceMock },
        // { provide: DEAL_SERVICE, useClass: DealServiceMock },
        // { provide: MOVEMENT_SERVICE, useClass: MovementServiceMock },
        // { provide: USER_SERVICE, useClass: UserServiceMock },
        { provide: AlertService, useClass: AlertService }
    ]
})
export class AppModule { }
