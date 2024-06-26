import { Routes } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { UserCreateComponent } from './components/user-create/user-create.component';
import { UserDetailComponent } from './components/user-detail/user-detail.component';
import { LoginComponent } from './components/login/login.component';
import { loginGuard } from './shared/guards/login.guard';
import { userGuard } from './shared/guards/user.guard';
import { createGuard } from './shared/guards/create.guard';

export const routes: Routes = [
  {
    path: 'users',
    component: UserListComponent,
    canActivate: [userGuard],
  },
  {
    path: 'users/:id',
    component: UserDetailComponent,
    canActivate: [userGuard],
  },
  {
    path: 'create',
    component: UserCreateComponent,
    canActivate: [createGuard],
  },
  { path: 'login', component: LoginComponent, canActivate: [loginGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'page-not-found', component: PageNotFoundComponent },
];
