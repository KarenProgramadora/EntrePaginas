export interface Fine {
  id: number;
  loanId: number;
  amount: number;
  issuedDate: string;
  paidDate?: string;
  isPaid: boolean;
  notes?: string;
  createdAt: string;
  updatedAt?: string;
}
