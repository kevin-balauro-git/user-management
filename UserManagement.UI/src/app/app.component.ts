import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserDetailComponent } from './components/user-detail/user-detail.component';
import { HeaderComponent } from './components/header/header.component';
import { AuthService } from './services/auth.service';
import { UserApiService } from './services/user-api.service';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    UserListComponent,
    UserDetailComponent,
    HeaderComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  constructor(
    private userApiService: UserApiService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {}
}
