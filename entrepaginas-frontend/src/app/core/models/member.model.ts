export interface Member {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phone: string;
  membershipDate: string;
  status: number;
  statusName: string;
  createdAt: string;
  updatedAt?: string;
}

export interface MemberRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  status: number;
}
