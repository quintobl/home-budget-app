export interface Transaction {
  id: number;
  accountId: number;
  categoryId: number;
  description: string;
  amount: number;
  date: Date;
  type: 'debit' | 'credit';
  userId: number;
}
