import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { UserAuthService } from '../../services/user-auth.service';

export const loginGuard: CanActivateFn = (route, state) => {
  const userAuthService: UserAuthService = inject(UserAuthService);
  const router: Router = inject(Router);

  if (userAuthService.currentUserSig() === null) return true;
  else {
    router.navigateByUrl('/users');
    return false;
  }
};
