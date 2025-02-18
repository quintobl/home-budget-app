import { User } from './user';
import { Transaction } from './transaction';

export interface Account {
  id: number;
  name: string;
  balance: number;
  userid: User[];
  transactions?: Transaction[];
}
