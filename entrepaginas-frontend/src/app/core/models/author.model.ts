export interface Author {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  nationality: string;
  birthDate?: string;
  biography?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface AuthorRequest {
  firstName: string;
  lastName: string;
  nationality: string;
  birthDate?: string;
  biography?: string;
}
