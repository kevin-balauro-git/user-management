import { Component } from '@angular/core';

import { UserAuthService } from '../../services/user-auth.service';
import { AsyncPipe, JsonPipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [NgIf, JsonPipe, AsyncPipe],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  constructor(private userAuthService: UserAuthService) {}

  get currentUser() {
    return this.userAuthService.currentUserSig();
  }

  public logout(): void {
    this.userAuthService.logout();
  }

  public details(): void {}
}
