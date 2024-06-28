import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const user = localStorage.getItem('user') ?? '';

  if (user) {
    const token = JSON.parse(user).token;

    req = req.clone({
      setHeaders: {
        Authorization: token ? `Bearer ${token}` : '',
      },
    });
  }

  return next(req);
};
