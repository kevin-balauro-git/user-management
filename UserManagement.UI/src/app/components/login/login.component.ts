import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthUser } from '../../models/auth-user.interface';
import { JsonPipe, NgIf } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { BehaviorSubject } from 'rxjs';
import { UserApiService } from '../../services/user-api.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, JsonPipe],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  private loginForm: FormGroup = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });
  public errorStatus: any | undefined;

  public get form() {
    return this.loginForm;
  }

  public get email() {
    return this.loginForm.controls['email'];
  }

  public get password() {
    return this.loginForm.controls['password'];
  }
  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {}

  public employee(): void {
    this.email.setValue('fdagojoy@email.com');
    this.password.setValue('daG0nsa#uyu#oy');
    const employee = {
      email: 'fdagojoy@email.com',
      password: 'daG0nsa#uyu#oy',
    };
    setTimeout(() => this.login(employee), 300);
  }

  public admin(): void {
    this.email.setValue('juanluna@email.com');
    this.password.setValue('juanLun4uno*');
    const admin = {
      email: 'juanluna@email.com',
      password: 'juanLun4uno*',
    };
    setTimeout(() => this.login(admin), 300);
  }

  public login(formData: any): void {
    this.authService.login(formData).subscribe({
      next: (user) => {
        localStorage.setItem('user', JSON.stringify(user));
        this.router.navigateByUrl('/users');
      },
      error: (error) => {
        this.errorStatus = error;
        console.error(error);
      },
    });
  }

  public back(): void {
    this.router.navigateByUrl('/');
  }
}
