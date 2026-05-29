import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { NotificationService } from '../services/notification.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notifier = inject(NotificationService);

  return next(req).pipe(
    catchError(err => {
      const msg = err.error?.message ?? err.message ?? 'Error desconocido';
      console.error(`[HTTP ${err.status}] ${req.method} ${req.url} → ${msg}`);
      notifier.error(msg);
      return throwError(() => err);
    })
  );
};
