import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api-service';
import { Member, MemberRequest } from '../models/member.model';

@Injectable({ providedIn: 'root' })
export class MemberService {
  private api = inject(ApiService);

  getAll(): Observable<Member[]> { return this.api.get<Member[]>('member'); }
  getById(id: number): Observable<Member> { return this.api.get<Member>(`member/${id}`); }
  create(dto: MemberRequest): Observable<Member> { return this.api.post<Member>('member', dto); }
  update(id: number, dto: MemberRequest): Observable<void> { return this.api.put<void>(`member/${id}`, dto); }
  delete(id: number): Observable<void> { return this.api.delete<void>(`member/${id}`); }
}
