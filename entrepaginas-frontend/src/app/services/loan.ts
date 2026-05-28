import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Loan, LoanRequest } from '../models/models';

@Injectable({ providedIn: 'root' })
export class LoanService {
  private url = `${environment.apiUrl}/loan`;
  constructor(private http: HttpClient) {}

  getAll(): Observable<Loan[]> { return this.http.get<Loan[]>(this.url); }
  getById(id: number): Observable<Loan> { return this.http.get<Loan>(`${this.url}/${id}`); }
  getOverdue(): Observable<Loan[]> { return this.http.get<Loan[]>(`${this.url}/overdue`); }
  getByMember(memberId: number): Observable<Loan[]> { return this.http.get<Loan[]>(`${this.url}/member/${memberId}`); }
  create(dto: LoanRequest): Observable<Loan> { return this.http.post<Loan>(this.url, dto); }
  returnLoan(id: number): Observable<void> { return this.http.patch<void>(`${this.url}/${id}/return`, {}); }
  markOverdue(): Observable<void> { return this.http.post<void>(`${this.url}/mark-overdue`, {}); }
}
