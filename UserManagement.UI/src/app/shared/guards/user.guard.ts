import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { inject } from '@angular/core';

export const userGuard: CanActivateFn = (route, state) => {
  const authService: AuthService = inject(AuthService);
  const router: Router = inject(Router);

  if (authService.userValue) {
    console.log(authService.userValue.isAdmin);
    return true;
  } else {
    router.navigateByUrl('/');
    return false;
  }
};
