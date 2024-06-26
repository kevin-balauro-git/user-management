import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { inject } from '@angular/core';

export const loginGuard: CanActivateFn = (route, state) => {
  const authService: AuthService = inject(AuthService);
  const router: Router = inject(Router);

  console.log(authService.userValue);
  if (authService.userValue === null) return true;
  else {
    router.navigateByUrl('/users');
    return false;
  }
};
