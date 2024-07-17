import { CanActivateFn, Router } from '@angular/router';

import { inject } from '@angular/core';
import { UserAuthService } from '../../services/user-auth.service';

export const userGuard: CanActivateFn = (route, state) => {
  const router: Router = inject(Router);
  const userAuthService: UserAuthService = inject(UserAuthService);

  if (userAuthService.currentUserSig()) {
    return true;
  } else {
    router.navigateByUrl('/login');
    return false;
  }
};
