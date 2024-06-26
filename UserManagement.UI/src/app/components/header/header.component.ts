import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AsyncPipe, JsonPipe, NgIf } from '@angular/common';
import { AuthUser } from '../../models/auth-user.interface';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [NgIf, JsonPipe, AsyncPipe],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  public currentUser$: Observable<AuthUser | null> | undefined;
  public currentUser: AuthUser | null | undefined;
  constructor(private router: Router, private authService: AuthService) {}

  public getCurrentUser() {}
  public ngOnInit(): void {
    this.authService.currentUser.subscribe((data) => {
      this.currentUser = data;
    });
  }

  public login(): void {
    this.router.navigateByUrl('login');
  }

  public logout(): void {
    this.authService.logout();
  }
}
