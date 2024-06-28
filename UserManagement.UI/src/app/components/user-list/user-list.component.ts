import { Component, OnInit } from '@angular/core';
import { UserApiService } from '../../services/user-api.service';
import { User } from '../../models/user.interface';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [NgFor, NgIf, AsyncPipe, ReactiveFormsModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css',
})
export class UserListComponent implements OnInit {
  private hasDeleted: boolean = false;
  public form: FormGroup = this.formbuilder.group({
    searchItem: [''],
  });
  private userList$: Observable<User[]> | undefined;

  constructor(
    private userApiService: UserApiService,
    private router: Router,
    private authService: AuthService,
    private formbuilder: FormBuilder
  ) {}

  get deleted(): boolean {
    return this.hasDeleted;
  }

  get searchItem() {
    return this.form.controls['searchItem'];
  }

  public getUserListObs(): Observable<User[]> | undefined {
    return this.userList$;
  }

  public isAdmin(): boolean {
    if (this.authService.userValue?.isAdmin.toLowerCase() === 'true') {
      return true;
    } else return false;
  }

  public hasLogin(): boolean {
    if (this.authService.userValue) return true;
    else return false;
  }

  ngOnInit(): void {
    this.userList$ = this.userApiService.getUsers();
  }

  public showDetails(id: number): void {
    this.router.navigateByUrl(`users/${id}`);
  }

  public onCreateUser(): void {
    this.router.navigateByUrl(`/create`);
  }

  public onDeleteUser(id: number): void {
    if (confirm('Are you sure to delete this user?')) {
      this.userApiService.deleteUser(id).subscribe();
      this.hasDeleted = true;

      setTimeout(() => {
        this.hasDeleted = false;
        this.userList$ = this.userApiService.getUsers();
      }, 700);
    }
  }

  public search(item: any): void {
    this.userList$ = this.userApiService.getUsers(item.searchItem);
  }
}
