import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserApiService } from '../../services/user-api.service';
import { User } from '../../models/user.interface';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { UserAuthService } from '../../services/user-auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [NgFor, NgIf, AsyncPipe, ReactiveFormsModule],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css',
})
export class UserListComponent implements OnInit, OnDestroy {
  private hasDeleted: boolean = false;
  private form: FormGroup = this.formbuilder.group({ searchItem: [''] });
  private userList$: Observable<User[]> | undefined;
  private users: User[] | undefined;
  private usersSub$: Subscription | undefined;
  public isDesc: boolean = true;

  constructor(
    private userApiService: UserApiService,
    private router: Router,
    private userAuthService: UserAuthService,
    private formbuilder: FormBuilder
  ) {}

  get searchForm() {
    return this.form;
  }

  get deleted(): boolean {
    return this.hasDeleted;
  }

  get searchItem() {
    return this.form.controls['searchItem'];
  }

  get usersList() {
    return this.users;
  }

  public getUserListObs(): Observable<User[]> | undefined {
    return this.userList$;
  }

  public hasLogin(): boolean {
    return true;
  }

  ngOnInit(): void {
    this.usersSub$ = this.userApiService.getUsers().subscribe({
      next: (data) => {
        this.users = data;
      },
      error: (err) => {},
    });
  }

  ngOnDestroy(): void {
    this.usersSub$?.unsubscribe();
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
        this.usersSub$?.unsubscribe();
        this.usersSub$ = this.userApiService.getUsers().subscribe({
          next: (data) => (this.users = data),
          error: (err) => {},
        });
      }, 700);
    }
  }

  public search(item: any): void {
    this.usersSub$?.unsubscribe();
    this.usersSub$ = this.userApiService.getUsers(item.searchItem).subscribe({
      next: (data) => (this.users = data),
      error: (err) => {},
    });
  }

  public sort(sortName: string): void {
    this.isDesc = !this.isDesc;
    this.usersSub$?.unsubscribe();
    this.usersSub$ = this.userApiService
      .getUsers('', this.isDesc === true ? 'desc' : 'asc')
      .subscribe({
        next: (data) => (this.users = data),
        error: (err) => {},
      });
  }

  public sortImage() {
    if (this.isDesc) return '../../../assets/img/desc.png';
    return '../../../assets/img/asc.png';
  }
}
