import {EventEmitter, Injectable} from '@angular/core';
import {AlertComponent} from 'ng2-bootstrap/ng2-bootstrap';

@Injectable()
export class AlertService {
    newAlert: EventEmitter<AlertComponent> = new EventEmitter<AlertComponent>();
}
