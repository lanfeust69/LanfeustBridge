import {Component, Inject, provide} from 'angular2/core';
import {RouteConfig, Router, ROUTER_DIRECTIVES, ROUTER_PROVIDERS} from 'angular2/router';
import {Alert, DATEPICKER_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {CliRouteConfig} from './route-config';
import {AlertService} from './alert.service';
import {TOURNAMENT_SERVICE, TournamentService} from './tournament.service';
import {TournamentServiceMock} from './tournament.service.mock';
import {TournamentComponent} from './tournament.component';
import {TournamentListComponent} from './tournament-list.component';
import {DEAL_SERVICE} from './deal.service';
import {DealServiceMock} from './deal.service.mock';
import {DealComponent} from './deal.component'

@Component({
    selector: 'lanfeust-bridge-app',
    templateUrl: 'app/lanfeust-bridge.html',
    providers: [
        ROUTER_PROVIDERS,
        AlertService,
        provide(DEAL_SERVICE, {useClass: DealServiceMock}),
        provide(TOURNAMENT_SERVICE, {useClass: TournamentServiceMock})],
    directives: [ROUTER_DIRECTIVES, Alert],
    pipes: []
})
@RouteConfig([
    { path:'/', name: 'TournamentList',  component: TournamentListComponent },
    { path:'/tournament/:id', name: 'Tournament',  component: TournamentComponent },
    { path:'/new-tournament', name: 'NewTournament',  component: TournamentComponent }
].concat(CliRouteConfig))
export class LanfeustBridgeApp {

    constructor(private _router: Router, private _alertService: AlertService) {}
    
    ngOnInit() {
        this._alertService.newAlert.subscribe(alert => this.alerts.push(alert));
    }

    randomDeal: number = Math.floor(Math.random() * 32) + 1;
    alerts: Object[] = [];
    public addAlert() {
        this.alerts.push({msg: 'Another alert!', type: 'warning', dismissible: true});
    }
    public closeAlert(i:number):void {
        this.alerts.splice(i, 1);
    }
}
