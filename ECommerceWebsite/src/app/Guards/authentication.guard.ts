import { Injectable, inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Injectable({providedIn: 'root'})
export class AuthenticationService {

  constructor(
  private cookieService: CookieService,
  public router: Router,
  ) { }

  canActivate(): boolean {
    if (this.cookieService.get('Token')) {
      if ((localStorage.getItem("Role") && localStorage.getItem("Role") == "User") || (this.cookieService.get("Role") && this.cookieService.get("Role") == "User")) {
        return true
      }
      return false;
    } else {
      this.router.navigate(['/login']);
      return false
    }
  }

}

export const authenticationGuard: CanActivateFn = (route, state) => {
  return inject(AuthenticationService).canActivate();
};
