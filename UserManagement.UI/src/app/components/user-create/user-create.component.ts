import { JsonPipe, Location, NgIf } from '@angular/common';
import { AfterViewInit, Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import L from 'leaflet';
import { PasswordValidator } from '../../shared/validation/password.validator';
import { ConfirmPasswordValidator } from '../../shared/validation/confirmPassword.validator';
import { UserApiService } from '../../services/user-api.service';
import { Router } from '@angular/router';
import { MapService } from '../../services/map.service';

@Component({
  selector: 'app-user-create',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, JsonPipe],
  templateUrl: './user-create.component.html',
  styleUrl: './user-create.component.css',
})
export class UserCreateComponent implements AfterViewInit {
  private showPassword: boolean = false;

  private createUserForm: FormGroup = this.formBuilder.group({
    name: this.formBuilder.nonNullable.group({
      firstName: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.pattern('^[a-zA-Z]+$'),
        ],
      ],
      lastName: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.pattern('^[a-zA-Z]+$'),
        ],
      ],
    }),

    username: ['', [Validators.required, Validators.minLength(6)]],
    passGroup: this.formBuilder.nonNullable.group({
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          PasswordValidator.isWeak,
          PasswordValidator.hasSpace,
        ],
      ],
      confirmPassword: [
        '',
        [Validators.required, ConfirmPasswordValidator.isSame()],
      ],
    }),
    email: ['', [Validators.required, Validators.email]],
    phone: [''],
    address: this.formBuilder.nonNullable.group({
      streetName: [''],
      streetNumber: [''],
      city: [''],
      zipCode: [''],
      geoLocation: this.formBuilder.nonNullable.group({
        latitude: [''],
        longitude: [''],
      }),
    }),
  });

  public errorCode: any;

  constructor(
    private location: Location,
    private formBuilder: FormBuilder,
    private router: Router,
    private userApiService: UserApiService,
    private mapService: MapService
  ) {}

  public ngAfterViewInit(): void {
    this.initMap();
  }

  get form() {
    return this.createUserForm;
  }

  get firstName() {
    return this.createUserForm.controls['name'].get('firstName');
  }

  get lastName() {
    return this.createUserForm.controls['name'].get('lastName');
  }

  get username() {
    return this.createUserForm.get('username');
  }

  get email() {
    return this.createUserForm.get('email');
  }

  get confirmPassword() {
    return this.createUserForm.controls['passGroup'].get('confirmPassword');
  }

  get password() {
    return this.createUserForm.controls['passGroup'].get('password');
  }

  private initMap(): void {
    this.mapService.createMap();
    this.mapService.createTileLayer();
    this.mapService.markPosition(
      this.createUserForm.controls['address'].get('geoLocation.latitude'),
      this.createUserForm.controls['address'].get('geoLocation.longitude')
    );
    this.mapService.mapAddToTiles();
  }

  public hasShowPassword(): boolean {
    return this.showPassword;
  }

  public onCreateUser(formData: any): void {
    const newUser = {
      id: 0,
      name: {
        firstName: formData.name.firstName,
        lastName: formData.name.lastName,
      },
      username: formData.username,
      email: formData.email,
      phone: formData.phone,
      password: formData.passGroup.password,
      address: {
        city: formData.address.city,
        streetName: formData.address.streetName,
        streetNumber: formData.address.streetNumber,
        zipCode: formData.address.zipCode,
        geoLocation: {
          latitude: formData.address.geoLocation.latitude.toString(),
          longitude: formData.address.geoLocation.longitude.toString(),
        },
      },
      isAdmin: 'false',
    };

    this.userApiService.createUser(newUser).subscribe({
      error: (error) => {
        window.scrollTo(0, 0);
        this.errorCode = error.error;
      },
      complete: () => {
        this.router.navigateByUrl('/users');
      },
    });
  }

  public populateData(): void {
    if (this.mapService.Marker)
      this.mapService.Map.removeLayer(this.mapService.Marker);
    this.createUserForm.controls['name'].get('firstName')?.setValue('Protacio');
    this.createUserForm.controls['name'].get('lastName')?.setValue('Mercado');
    this.createUserForm.controls['username'].setValue('joserizz-al');
    this.createUserForm.controls['email'].setValue('joserizal@email.com');
    this.createUserForm.controls['phone'].setValue('07031892');
    this.createUserForm.controls['passGroup']
      .get('password')
      ?.setValue('laliP1lipin@s');
    this.createUserForm.controls['passGroup']
      .get('confirmPassword')
      ?.setValue('laliP1lipin@s');
    this.createUserForm.controls['address'].get('city')?.setValue('Calamba');
    this.createUserForm.controls['address']
      .get('streetName')
      ?.setValue('Francisco Mercado ');
    this.createUserForm.controls['address'].get('streetNumber')?.setValue('5');
    this.createUserForm.controls['address'].get('zipCode')?.setValue('4028');

    this.mapService.markPosition(
      this.createUserForm.controls['address'].get('geoLocation.latitude'),
      this.createUserForm.controls['address'].get('geoLocation.longitude')
    );
    this.mapService.createMarker(14.5826, 120.9787);
    this.mapService.Marker.addTo(this.mapService.Map);

    window.scrollTo(0, 0);
  }

  public back(): void {
    this.location.back();
  }

  public show(): void {
    this.showPassword = !this.showPassword;
  }
}
