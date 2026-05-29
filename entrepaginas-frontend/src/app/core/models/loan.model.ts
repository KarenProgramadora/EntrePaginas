export interface Loan {
  id: number;
  bookId: number;
  bookTitle: string;
  memberId: number;
  memberFullName: string;
  loanDate: string;
  dueDate: string;
  returnDate?: string;
  status: number;
  statusName: string;
  isOverdue: boolean;
  createdAt: string;
  updatedAt?: string;
}

export interface LoanRequest {
  memberId: number;
  bookId: number;
  loanDays: number;
}
