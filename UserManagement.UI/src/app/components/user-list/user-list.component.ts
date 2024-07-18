import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserApiService } from '../../services/user-api.service';
import { User } from '../../models/user.interface';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { UserAuthService } from '../../services/user-auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Data } from '../../models/data.interface';
import { Pagination } from '../../models/pagination.interface';

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

  private users: User[] | undefined;
  private pagination: Pagination | undefined;
  private usersSub$: Subscription | undefined;
  private currentNumber: number = 0;
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

  get pageNumber() {
    return Array<number>(
      Math.ceil(this.pagination?.totalCount! / this.pagination?.pageSize!)
    );
  }

  public hasLogin(): boolean {
    return true;
  }

  public getUsers(searchItem: string, sortOrder: string, pageNumber: number) {
    this.usersSub$ = this.userApiService
      .getUsers(searchItem, sortOrder, pageNumber)
      .subscribe({
        next: (data: Data) => {
          this.pagination = data.pagination;
          this.pageNumber;
          this.users = data.users;
        },
        error: (err) => {},
      });
  }
  ngOnInit(): void {
    this.getUsers('', 'desc', 0);
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
        this.getUsers('', 'desc', 0);
      }, 700);
    }
  }

  public search(item: any): void {
    this.usersSub$?.unsubscribe();
    this.getUsers(item.searchItem, 'desc', 0);
  }

  public sort(sortName: string): void {
    this.isDesc = !this.isDesc;
    this.usersSub$?.unsubscribe();
    this.getUsers('', this.isDesc === true ? 'desc' : 'asc', 0);
  }

  public sortImage() {
    if (this.isDesc) return '../../../assets/img/desc.png';
    return '../../../assets/img/asc.png';
  }

  public page(n: number) {
    this.currentNumber = n;
    this.getUsers('', 'desc', this.currentNumber);
  }

  public previous() {
    if (this.currentNumber < 0) return;
    this.currentNumber = this.currentNumber - 1;
    this.getUsers('', 'desc', this.currentNumber);
  }

  public next() {
    if (this.currentNumber < this.pageNumber.length - 1) {
      this.currentNumber = this.currentNumber + 1;
      this.getUsers('', 'desc', this.currentNumber);
    }
  }
}
