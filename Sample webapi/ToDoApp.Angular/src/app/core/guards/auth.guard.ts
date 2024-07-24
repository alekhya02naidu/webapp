import { AuthenticationService } from '../services/authentication.service';
import { ActivatedRouteSnapshot, CanActivateFn, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Inject } from '@angular/core';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree => {
  const authService = Inject(AuthenticationService);

  if(authService.isLoggedIn()) {
    return true;
  }
  else {
    const redirectUrl = '/sign-in';
    return authService.router.createUrlTree([redirectUrl]);
  }
};