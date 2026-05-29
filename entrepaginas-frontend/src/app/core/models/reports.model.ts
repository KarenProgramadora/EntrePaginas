export interface LibraryStats {
  totalBooks: number;
  totalAvailableCopies: number;
  totalLoanedCopies: number;
  totalAuthors: number;
  totalMembers: number;
  activeMembers: number;
  activeLoans: number;
  overdueLoans: number;
  unpaidFines: number;
  totalUnpaidFinesAmount: number;
}

export interface BooksByCategory {
  categoryName: string;
  bookCount: number;
  totalCopies: number;
  availableCopies: number;
}

export interface MostLoanedBook {
  bookId: number;
  title: string;
  authorName: string;
  loanCount: number;
}

export interface MemberActivity {
  memberId: number;
  fullName: string;
  totalLoans: number;
  activeLoans: number;
  overdueLoans: number;
  totalFines: number;
}
