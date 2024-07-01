import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AsyncPipe, JsonPipe, NgIf } from '@angular/common';
import { AuthUser } from '../../models/auth-user.interface';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [NgIf, JsonPipe, AsyncPipe],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit, OnDestroy {
  private currentUser: AuthUser | null | undefined;
  private currentUserSub: Subscription | undefined;

  constructor(private router: Router, private authService: AuthService) {}

  public ngOnInit(): void {
    this.currentUserSub = this.authService.currentUser.subscribe((data) => {
      this.currentUser = data;
    });
  }

  public ngOnDestroy(): void {
    this.currentUserSub?.unsubscribe();
  }

  public get user() {
    return this.currentUser;
  }

  public login(): void {
    this.router.navigateByUrl('login');
  }

  public logout(): void {
    this.authService.logout();
  }
}
