import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { JsonPipe, NgIf } from '@angular/common';
import { UserAuthService } from '../../services/user-auth.service';

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

  private showPassword: boolean = false;

  public errorStatus: any | undefined;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private userAuthService: UserAuthService
  ) {}

  public get form() {
    return this.loginForm;
  }

  public get email() {
    return this.loginForm.controls['email'];
  }

  public get password() {
    return this.loginForm.controls['password'];
  }

  public employee(): void {
    const employee = {
      email: 'fdagojoy@email.com',
      password: 'daG0nsa#uyu#oy',
    };
    this.email.setValue(employee.email);
    this.password.setValue(employee.password);
    setTimeout(() => this.login(employee), 300);
  }

  public admin(): void {
    const admin = {
      email: 'admin@email.com',
      password: 'Pa$$w0rd',
    };
    this.email.setValue(admin.email);
    this.password.setValue(admin.password);

    setTimeout(() => this.login(admin), 300);
  }

  public login(formData: any): void {
    this.userAuthService.login(formData).subscribe({
      next: (response) => {
        localStorage.setItem('user', JSON.stringify(response));
        this.userAuthService.currentUserSig.set(response);
        this.router.navigateByUrl('/users');
      },
      error: (error) => {
        this.errorStatus = error;
      },
    });
  }

  public hasShowPassword(): boolean {
    return this.showPassword;
  }

  public show(): void {
    this.showPassword = !this.showPassword;
  }
}
