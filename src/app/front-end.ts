import {Component} from 'angular2/core';
import {RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS} from 'angular2/router';
import {CliRouteConfig} from './route-config';
import {Alert, DATEPICKER_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {DealComponent} from './deal.component'

@Component({
  selector: 'front-end-app',
  providers: [ROUTER_PROVIDERS],
  templateUrl: 'app/front-end.html',
  directives: [ROUTER_DIRECTIVES, Alert, DealComponent],
  pipes: []
})
@RouteConfig([

].concat(CliRouteConfig))

export class FrontEndApp {
  defaultMeaning: number = 42;

  meaningOfLife(meaning?: number) {
    return `The meaning of life is ${meaning || this.defaultMeaning}`;
  }
}
