import { Address } from './address.interface';
import { Name } from './name.interface';

export interface User {
  id: number;
  name: Name;
  username: string;
  email: string;
  password: string;
  phone: string;
  address: Address;
  isAdmin: string;
}
