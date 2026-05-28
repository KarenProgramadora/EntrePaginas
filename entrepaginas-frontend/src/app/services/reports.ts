import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LibraryStats, BooksByCategory, MostLoanedBook, MemberActivity } from '../models/models';

@Injectable({ providedIn: 'root' })
export class ReportsService {
  private url = `${environment.apiUrl}/reports`;
  constructor(private http: HttpClient) {}

  getStats(): Observable<LibraryStats> { return this.http.get<LibraryStats>(`${this.url}/stats`); }
  getBooksByCategory(): Observable<BooksByCategory[]> { return this.http.get<BooksByCategory[]>(`${this.url}/books-by-category`); }
  getMostLoaned(top = 5): Observable<MostLoanedBook[]> { return this.http.get<MostLoanedBook[]>(`${this.url}/most-loaned?top=${top}`); }
  getMemberActivity(): Observable<MemberActivity[]> { return this.http.get<MemberActivity[]>(`${this.url}/member-activity`); }
}
