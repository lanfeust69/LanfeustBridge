import {Component, provide} from 'angular2/core';
import {RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS} from 'angular2/router';
import {Alert, DATEPICKER_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {CliRouteConfig} from './route-config';
import {DEAL_SERVICE} from './deal.service';
import {DealServiceMock} from './deal.service.mock';
import {DealComponent} from './deal.component'

@Component({
    selector: 'front-end-app',
    providers: [ROUTER_PROVIDERS, provide(DEAL_SERVICE, {useClass: DealServiceMock})],
    templateUrl: 'app/front-end.html',
    directives: [ROUTER_DIRECTIVES, Alert, DealComponent],
    pipes: []
})
@RouteConfig([

].concat(CliRouteConfig))
export class FrontEndApp {
    randomDeal: number = Math.floor(Math.random() * 32);
    alerts: Object[] = [];
    public addAlert() {
        this.alerts.push({msg: 'Another alert!', type: 'warning', dismissible: true});
    }
    public closeAlert(i:number):void {
        this.alerts.splice(i, 1);
    }
    defaultMeaning: number = 42;

    meaningOfLife(meaning?: number) {
        return `The meaning of life is ${meaning || this.defaultMeaning}`;
    }
}
