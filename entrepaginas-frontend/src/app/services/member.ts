import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Member, MemberRequest } from '../models/models';

@Injectable({ providedIn: 'root' })
export class MemberService {
  private url = `${environment.apiUrl}/member`;
  constructor(private http: HttpClient) {}

  getAll(): Observable<Member[]> { return this.http.get<Member[]>(this.url); }
  getById(id: number): Observable<Member> { return this.http.get<Member>(`${this.url}/${id}`); }
  create(dto: MemberRequest): Observable<Member> { return this.http.post<Member>(this.url, dto); }
  update(id: number, dto: MemberRequest): Observable<void> { return this.http.put<void>(`${this.url}/${id}`, dto); }
  delete(id: number): Observable<void> { return this.http.delete<void>(`${this.url}/${id}`); }
}
