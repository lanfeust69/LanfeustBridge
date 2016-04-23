import {Component, provide} from 'angular2/core';
import {RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS} from 'angular2/router';
import {Alert, DATEPICKER_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {CliRouteConfig} from './route-config';
import {TOURNAMENT_SERVICE} from './tournament.service';
import {TournamentServiceMock} from './tournament.service.mock';
import {TournamentComponent} from './tournament.component';
import {DEAL_SERVICE} from './deal.service';
import {DealServiceMock} from './deal.service.mock';
import {DealComponent} from './deal.component'

@Component({
    selector: 'lanfeust-bridge-app',
    providers: [
        ROUTER_PROVIDERS,
        provide(TOURNAMENT_SERVICE, {useClass: TournamentServiceMock}),
        provide(DEAL_SERVICE, {useClass: DealServiceMock})],
    templateUrl: 'app/lanfeust-bridge.html',
    directives: [ROUTER_DIRECTIVES, Alert, TournamentComponent, DealComponent],
    pipes: []
})
@RouteConfig([

].concat(CliRouteConfig))
export class LanfeustBridgeApp {
    _tournamentNames: String[] = ["qsdf", "tklhj"];
    
    public createTournament() {
        alert("created !");
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
