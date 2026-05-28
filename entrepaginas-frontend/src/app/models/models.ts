export interface Category {
  id: number; name: string; description: string;
  createdAt: string; updatedAt?: string;
}

export interface Publisher {
  id: number; name: string; country: string;
  email?: string; website?: string;
  createdAt: string; updatedAt?: string;
}

export interface Author {
  id: number; firstName: string; lastName: string; fullName: string;
  nationality: string; birthDate?: string; biography?: string;
  createdAt: string; updatedAt?: string;
}

export interface Book {
  id: number; title: string; isbn: string;
  publicationYear: number; totalCopies: number; availableCopies: number;
  condition: number; conditionName: string; coverUrl?: string;
  categoryId: number; categoryName: string;
  publisherId: number; publisherName: string;
  createdAt: string; updatedAt?: string;
}

export interface Member {
  id: number; firstName: string; lastName: string; fullName: string;
  email: string; phone: string; membershipDate: string;
  status: number; statusName: string;
  createdAt: string; updatedAt?: string;
}

export interface Loan {
  id: number; bookId: number; bookTitle: string;
  memberId: number; memberFullName: string;
  loanDate: string; dueDate: string; returnDate?: string;
  status: number; statusName: string; isOverdue: boolean;
  createdAt: string; updatedAt?: string;
}

export interface Fine {
  id: number; loanId: number; amount: number;
  issuedDate: string; paidDate?: string; isPaid: boolean; notes?: string;
  createdAt: string; updatedAt?: string;
}

export interface LibraryStats {
  totalBooks: number; totalAvailableCopies: number; totalLoanedCopies: number;
  totalAuthors: number; totalMembers: number; activeMembers: number;
  activeLoans: number; overdueLoans: number;
  unpaidFines: number; totalUnpaidFinesAmount: number;
}

export interface BooksByCategory {
  categoryName: string; bookCount: number;
  totalCopies: number; availableCopies: number;
}

export interface MostLoanedBook {
  bookId: number; title: string; authorName: string; loanCount: number;
}

export interface MemberActivity {
  memberId: number; fullName: string;
  totalLoans: number; activeLoans: number; overdueLoans: number; totalFines: number;
}

// Request DTOs
export interface BookRequest {
  title: string; isbn: string; publicationYear: number;
  totalCopies: number; availableCopies: number; condition: number;
  coverUrl?: string; categoryId: number; publisherId: number;
}

export interface AuthorRequest {
  firstName: string; lastName: string; nationality: string;
  birthDate?: string; biography?: string;
}

export interface MemberRequest {
  firstName: string; lastName: string; email: string; phone: string; status: number;
}

export interface LoanRequest {
  memberId: number; bookId: number; loanDays: number;
}
