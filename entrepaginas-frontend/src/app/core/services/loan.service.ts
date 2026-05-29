import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api-service';
import { Loan, LoanRequest } from '../models/loan.model';

@Injectable({ providedIn: 'root' })
export class LoanService {
  private api = inject(ApiService);

  getAll(): Observable<Loan[]> { return this.api.get<Loan[]>('loan'); }
  getById(id: number): Observable<Loan> { return this.api.get<Loan>(`loan/${id}`); }
  getOverdue(): Observable<Loan[]> { return this.api.get<Loan[]>('loan/overdue'); }
  getByMember(memberId: number): Observable<Loan[]> { return this.api.get<Loan[]>(`loan/member/${memberId}`); }
  create(dto: LoanRequest): Observable<Loan> { return this.api.post<Loan>('loan', dto); }
  returnLoan(id: number): Observable<void> { return this.api.patch<void>(`loan/${id}/return`, {}); }
  markOverdue(): Observable<void> { return this.api.post<void>('loan/mark-overdue', {}); }
}
