import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

/**
 * DateInterceptor replaces fields named 'date' with Date values parsed from the expected ISO 8601 string
 */
@Injectable()
export class DateInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            map((event: HttpEvent<any>) => {
                if (event instanceof HttpResponse && event.body && event.body.date)
                    // note that while event is immutable, event.body isn't, and can be patched directly
                    event.body.date = new Date(event.body.date);
                return event;
            }));
    }
}
