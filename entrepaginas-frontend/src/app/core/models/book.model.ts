export interface Book {
  id: number;
  title: string;
  isbn: string;
  publicationYear: number;
  totalCopies: number;
  availableCopies: number;
  condition: number;
  conditionName: string;
  coverUrl?: string;
  categoryId: number;
  categoryName: string;
  publisherId: number;
  publisherName: string;
  createdAt: string;
  updatedAt?: string;
}

export interface BookRequest {
  title: string;
  isbn: string;
  publicationYear: number;
  totalCopies: number;
  availableCopies: number;
  condition: number;
  coverUrl?: string;
  categoryId: number;
  publisherId: number;
}
