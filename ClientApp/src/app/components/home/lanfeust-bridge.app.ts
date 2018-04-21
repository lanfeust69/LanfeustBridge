import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Alert, AlertService } from '../../services/alert/alert.service';
import { USER_SERVICE, UserService } from '../../services/user/user.service';

@Component({
    selector: 'lanfeust-bridge-app',
    templateUrl: './lanfeust-bridge.html'
})
// tslint:disable-next-line:component-class-suffix
export class LanfeustBridgeApp implements OnInit {
    randomDeal: number = Math.floor(Math.random() * 32) + 1;
    alerts: Object[] = [];
    user = '';

    constructor(private _router: Router,
        @Inject(USER_SERVICE) private _userService: UserService,
        private _alertService: AlertService) {}

    ngOnInit() {
        this._alertService.newAlert.subscribe(alert => this.alerts.push(alert));
        this._userService.isLoggedIn()
            .subscribe(b => {
                if (b) {
                    this.user = this._userService.currentUser;
                } else {
                    console.log('trying to navigate to login page');
                    window.location.href = '/Identity/Account/Login';
                }
            });
    }

    public addAlert() {
        this.alerts.push({msg: 'Another alert!', type: 'warning', dismissible: true});
    }

    public closeAlert(i: number): void {
        this.alerts.splice(i, 1);
    }
}
