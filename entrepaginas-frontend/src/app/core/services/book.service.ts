import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api-service';
import { Book, BookRequest } from '../models/book.model';

@Injectable({ providedIn: 'root' })
export class BookService {
  private api = inject(ApiService);

  getAll(): Observable<Book[]> { return this.api.get<Book[]>('book'); }
  getById(id: number): Observable<Book> { return this.api.get<Book>(`book/${id}`); }
  getAvailable(): Observable<Book[]> { return this.api.get<Book[]>('book/available'); }
  create(dto: BookRequest): Observable<Book> { return this.api.post<Book>('book', dto); }
  update(id: number, dto: BookRequest): Observable<void> { return this.api.put<void>(`book/${id}`, dto); }
  delete(id: number): Observable<void> { return this.api.delete<void>(`book/${id}`); }
}
