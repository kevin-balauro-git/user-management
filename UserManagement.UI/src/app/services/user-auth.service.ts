import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { UserAuth } from '../models/user-auth.interface';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { UserRole } from '../models/user-role.interface';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class UserAuthService {
  private readonly baseUrl: string = environment.authApi;

  public currentUserSig = signal<UserAuth | null | undefined>(undefined);

  constructor(private http: HttpClient, private router: Router) {
    this.currentUserSig.set(JSON.parse(localStorage.getItem('user')!));
  }

  public login(user: any): Observable<UserAuth> {
    return this.http.post<any>(`${this.baseUrl}/login`, user);
  }

  public logout() {
    localStorage.removeItem('user');
    this.currentUserSig.set(null);
    this.router.navigateByUrl('/login');
  }

  public getUserRole(): UserRole | null {
    const user = this.userToken();
    const token = user.token;

    const decodeToken = jwtDecode<UserRole>(token);
    return decodeToken;
  }

  public userToken() {
    return JSON.parse(localStorage.getItem('user')!);
  }
}
