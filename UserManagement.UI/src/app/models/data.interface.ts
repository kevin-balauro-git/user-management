import { Pagination } from './pagination.interface';
import { User } from './user.interface';

export interface Data {
  pagination: Pagination;
  users: User[];
}
