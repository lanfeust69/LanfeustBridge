import { EventEmitter, Injectable } from '@angular/core';

export interface Alert {
  type: string;
  dismissible: boolean;
  msg: string;
}

@Injectable()
export class AlertService {
    newAlert: EventEmitter<Alert> = new EventEmitter<Alert>();
}
