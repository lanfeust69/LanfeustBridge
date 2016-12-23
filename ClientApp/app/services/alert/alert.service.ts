import {EventEmitter, Injectable} from '@angular/core';

export interface Alert {
  id: number;
  type: string;
  message: string;
}

@Injectable()
export class AlertService {
    newAlert: EventEmitter<Alert> = new EventEmitter<Alert>();
}
