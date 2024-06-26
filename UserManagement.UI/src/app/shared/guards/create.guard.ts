import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

export const createGuard: CanActivateFn = (route, state) => {
  const authService: AuthService = inject(AuthService);
  const router: Router = inject(Router);

  if (authService.userValue && authService.userValue.isAdmin === 'true') {
    return true;
  } else {
    router.navigateByUrl('/');
    return false;
  }
};
