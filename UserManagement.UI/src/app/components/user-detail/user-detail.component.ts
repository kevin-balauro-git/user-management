import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserApiService } from '../../services/user-api.service';
import { User } from '../../models/user.interface';
import { AsyncPipe, Location, NgIf } from '@angular/common';
import * as L from 'leaflet';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { PasswordValidator } from '../../shared/validation/password.validator';
import { AuthService } from '../../services/auth.service';
import { MapService } from '../../services/map.service';

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [NgIf, AsyncPipe, ReactiveFormsModule],
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.css',
})
export class UserDetailComponent implements OnInit {
  private showPassword: boolean = false;
  private disable = true;
  private updateForm!: FormGroup;
  public errorStatus: any;
  private userId: number = 0;

  public hasShowPassword(): boolean {
    return this.showPassword;
  }

  constructor(
    private route: ActivatedRoute,
    private userApiService: UserApiService,
    private router: Router,
    private location: Location,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private mapService: MapService
  ) {}

  get form() {
    return this.updateForm;
  }

  get isDisabled() {
    return this.disable;
  }

  get isAdmin() {
    return this.authService.userValue?.isAdmin === 'true' ? true : false;
  }

  get firstName() {
    return this.updateForm.controls['name']?.get('firstName');
  }

  get lastName() {
    return this.updateForm.controls['name']?.get('lastName');
  }

  get username() {
    return this.updateForm.get('username');
  }

  get email() {
    return this.updateForm.get('email');
  }

  get password() {
    return this.updateForm.get('password');
  }

  get phone() {
    return this.updateForm.get('phone');
  }

  get admin() {
    return this.updateForm.get('isAdmin');
  }

  get city() {
    return this.updateForm.controls['address'].get('city');
  }

  get streetName() {
    return this.updateForm.controls['address'].get('streetName');
  }

  get streetNumber() {
    return this.updateForm.controls['address'].get('streetNumber');
  }

  get zipCode() {
    return this.updateForm.controls['address'].get('zipCode');
  }

  get latitude() {
    return this.updateForm.controls['address'].get('geolocation.latitude');
  }

  get longitude() {
    return this.updateForm.controls['address'].get('geolocation.longitude');
  }

  public ngOnInit(): void {
    this.onGetUser();
  }

  private initMap(latitude: number, longitude: number) {
    this.mapService.setCenter(latitude, longitude);
    this.mapService.createMap();
    this.mapService.disableMap();

    this.mapService.createMarker(latitude, longitude);
    this.mapService.Marker.addTo(this.mapService.Map);
    this.mapService.createTileLayer();

    this.mapService.markPosition(
      this.updateForm.controls['address'].get('geoLocation.latitude'),
      this.updateForm.controls['address'].get('geoLocation.longitude')
    );
    this.mapService.mapAddToTiles();
  }

  private onGetUser(): void {
    this.route.paramMap.subscribe((params) => {
      const userId = +params.get('id')!;
      this.userApiService.getUser(userId).subscribe({
        next: (userData: User) => {
          this.userId = userData.id;
          this.populateData(userData);
          this.initMap(
            +userData.address.geoLocation.latitude,
            +userData.address.geoLocation.longitude
          );
        },
        error: (error) => {
          this.router.navigateByUrl('/page-not-found');
        },
      });
    });
  }

  private populateData(user: User) {
    this.updateForm = this.formBuilder.nonNullable.group({
      name: this.formBuilder.nonNullable.group({
        firstName: [user.name.firstName],
        lastName: [user.name.lastName],
      }),

      username: [user.username, [Validators.required, Validators.minLength(6)]],
      password: [
        { value: user.password, disabled: true },
        [
          Validators.required,
          Validators.minLength(8),
          PasswordValidator.isWeak,
          PasswordValidator.hasSpace,
        ],
      ],

      email: [
        { value: user.email, disabled: true },
        [Validators.required, Validators.email],
      ],
      phone: [{ value: user.phone, disabled: true }],
      isAdmin: [{ value: user.isAdmin, disabled: true }],
      address: this.formBuilder.nonNullable.group({
        streetName: [{ value: user.address.streetName, disabled: true }],
        streetNumber: [{ value: user.address.streetNumber, disabled: true }],
        city: [{ value: user.address.city, disabled: true }],
        zipCode: [{ value: user.address.zipCode, disabled: true }],
        geoLocation: this.formBuilder.nonNullable.group({
          latitude: [user.address.geoLocation.latitude],
          longitude: [user.address.geoLocation.longitude],
        }),
      }),
    });
  }

  public edit() {
    this.disable = false;

    this.mapService.enableMap();
    this.updateForm.get('email')?.enable();
    this.updateForm.get('password')?.enable();
    this.updateForm.get('phone')?.enable();
    this.updateForm.get('isAdmin')?.enable();
    this.updateForm.controls['address'].get('streetName')?.enable();
    this.updateForm.controls['address'].get('streetNumber')?.enable();
    this.updateForm.controls['address'].get('city')?.enable();
    this.updateForm.controls['address'].get('zipCode')?.enable();
  }

  public onUpdateUser(data: any) {
    if (confirm('Are you sure to update this user?')) {
      setTimeout(() => {
        this.userApiService.updateUser(this.userId, data).subscribe({
          error: (error) => {
            this.errorStatus = error;
          },
          complete: () => this.router.navigateByUrl('/users'),
        });
      }, 700);
    }
  }

  public show(): void {
    this.showPassword = !this.showPassword;
  }

  public back(): void {
    this.location.back();
  }
}
