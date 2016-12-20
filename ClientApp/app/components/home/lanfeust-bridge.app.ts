import {Component, Inject} from '@angular/core';
import {Router, Routes} from '@angular/router';
import {HttpModule} from '@angular/http';
// import {AlertComponent} from 'ng2-bootstrap/ng2-bootstrap';
// import {AlertService} from '../../services/alert/alert.service';

@Component({
    selector: 'lanfeust-bridge-app',
    templateUrl: './lanfeust-bridge.html'
})
export class LanfeustBridgeApp {

    constructor(private _router: Router) {}
    // constructor(private _router: Router, private _alertService: AlertService) {}
    
    // ngOnInit() {
    //     this._alertService.newAlert.subscribe(alert => this.alerts.push(alert));
    // }

    randomDeal: number = Math.floor(Math.random() * 32) + 1;
    alerts: Object[] = [];
    public addAlert() {
        this.alerts.push({msg: 'Another alert!', type: 'warning', dismissible: true});
    }
    public closeAlert(i:number):void {
        this.alerts.splice(i, 1);
    }
}
