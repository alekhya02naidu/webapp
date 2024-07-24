import { HttpInterceptorFn } from '@angular/common/http';
import { Inject } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = Inject(AuthenticationService);
  const token = authService.getToken(); 
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
  return next (req);
};