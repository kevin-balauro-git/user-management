import { GeoLocation } from './geolocation.interface';

export interface Address {
  city: string;
  streetNumber: string;
  streetName: string;
  zipCode: string;
  geoLocation: GeoLocation;
}
