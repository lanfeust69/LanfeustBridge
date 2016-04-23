import {EventEmitter, Injectable} from 'angular2/core';
import {Alert, DATEPICKER_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';

@Injectable()
export class AlertService {
    newAlert: EventEmitter<Alert> = new EventEmitter;
}
