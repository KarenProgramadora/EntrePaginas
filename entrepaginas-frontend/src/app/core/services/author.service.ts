import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api-service';
import { Author, AuthorRequest } from '../models/author.model';

@Injectable({ providedIn: 'root' })
export class AuthorService {
  private api = inject(ApiService);

  getAll(): Observable<Author[]> { return this.api.get<Author[]>('author'); }
  getById(id: number): Observable<Author> { return this.api.get<Author>(`author/${id}`); }
  create(dto: AuthorRequest): Observable<Author> { return this.api.post<Author>('author', dto); }
  update(id: number, dto: AuthorRequest): Observable<void> { return this.api.put<void>(`author/${id}`, dto); }
  delete(id: number): Observable<void> { return this.api.delete<void>(`author/${id}`); }
}
