import { HttpInterceptorFn } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) =>
  next(req).pipe(
    catchError(err => {
      const msg = err.error?.message ?? err.message ?? 'Error desconocido';
      console.error(`[HTTP ${err.status}] ${req.method} ${req.url} → ${msg}`);
      return throwError(() => err);
    })
  );
