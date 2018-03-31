import { Component, Inject, OnInit } from '@angular/core';

import {Alert, AlertService} from '../../services/alert/alert.service';

@Component({
    selector: 'lanfeust-bridge-app',
    templateUrl: './lanfeust-bridge.html'
})
// tslint:disable-next-line:component-class-suffix
export class LanfeustBridgeApp implements OnInit {
    randomDeal: number = Math.floor(Math.random() * 32) + 1;
    alerts: Object[] = [];

    constructor(private _alertService: AlertService) {}

    ngOnInit() {
        this._alertService.newAlert.subscribe(alert => this.alerts.push(alert));
    }

    public addAlert() {
        this.alerts.push({msg: 'Another alert!', type: 'warning', dismissible: true});
    }

    public closeAlert(i: number): void {
        this.alerts.splice(i, 1);
    }
}
