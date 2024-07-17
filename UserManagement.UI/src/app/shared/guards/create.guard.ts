import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserAuthService } from '../../services/user-auth.service';

export const createGuard: CanActivateFn = (route, state) => {
  const userAuthService: UserAuthService = inject(UserAuthService);
  const router: Router = inject(Router);

  if (!userAuthService.userToken()) {
    router.navigateByUrl('/login');
    return false;
  }

  const role: any = userAuthService.getUserRole()?.role;
  let admin: string[] = [];

  if (typeof role === 'object')
    admin = role.filter(
      (r: string) =>
        r.toLowerCase() === 'admin' || r.toLowerCase() === 'moderator'
    );
  else {
    if (
      role.toString().toLowerCase() === 'admin' ||
      role.toString().toLowerCase() === 'moderator'
    )
      admin.push(role.toString());
  }

  if (admin?.length > 0) return true;
  else {
    router.navigateByUrl('/login');
    return false;
  }
};
