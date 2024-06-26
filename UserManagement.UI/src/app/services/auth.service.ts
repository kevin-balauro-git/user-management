import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthUser } from '../models/auth-user.interface';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly baseUrl: string = 'https://localhost:7169/api/account';

  private currentUserSubject: BehaviorSubject<AuthUser | null>;
  public currentUser: Observable<AuthUser | null>;

  constructor(private http: HttpClient, private router: Router) {
    this.currentUserSubject = new BehaviorSubject(
      JSON.parse(localStorage.getItem('user')!)
    );
    this.currentUser = this.currentUserSubject.asObservable();
  }

  get userValue() {
    return this.currentUserSubject.value;
  }

  public login(user: any): Observable<AuthUser> {
    return this.http.post<AuthUser>(`${this.baseUrl}/login`, user).pipe(
      map((u) => {
        this.currentUserSubject.next(u);
        return u;
      })
    );
  }

  public logout() {
    localStorage.removeItem('user');
    this.currentUserSubject.next(null);
    this.router.navigateByUrl('/');
  }
}
