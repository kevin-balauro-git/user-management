import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, filter } from 'rxjs';
import { User } from '../models/user.interface';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserApiService {
  private readonly baseUrl: string = environment.usersApi;

  constructor(private http: HttpClient) {}

  public getUsers(
    searchItem: string = '',
    sortOrder: string = 'desc'
  ): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseUrl}`, {
      params: { searchItem: searchItem, sortOrder: sortOrder },
    });
  }

  public getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/${id}`);
  }

  public createUser(newUser: User): Observable<User> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post<User>(this.baseUrl, newUser, {
      headers,
    });
  }

  public updateUser(id: number, updatedUser: any): Observable<User> {
    return this.http.put<User>(`${this.baseUrl}/${id}`, updatedUser);
  }

  public deleteUser(id: number): Observable<User> {
    return this.http.delete<User>(`${this.baseUrl}/${id.toString()}`);
  }
}
