import { Address } from './address.interface';
import { Name } from './name.interface';

export interface User {
  id: number;
  name: Name;
  userName: string;
  email: string;
  password: string;
  phoneNumber: string;
  address: Address;
}
