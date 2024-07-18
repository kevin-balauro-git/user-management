import { Component } from '@angular/core';

import { UserAuthService } from '../../services/user-auth.service';
import { AsyncPipe, JsonPipe, NgIf } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { UserRole } from '../../models/user-role.interface';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [NgIf, JsonPipe, AsyncPipe],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  constructor(
    private router: Router,
    private userAuthService: UserAuthService
  ) {}

  get currentUser() {
    return this.userAuthService.currentUserSig();
  }

  public logout(): void {
    this.userAuthService.logout();
  }

  public details(): void {
    const userToken = this.userAuthService.userToken();
    if (!userToken) return;
    const decodeToken = jwtDecode<UserRole>(userToken.token);
    this.router.navigateByUrl(`users/${decodeToken.nameid}`);
  }
}
