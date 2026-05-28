import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Book, BookRequest } from '../models/models';

@Injectable({ providedIn: 'root' })
export class BookService {
  private url = `${environment.apiUrl}/book`;
  constructor(private http: HttpClient) {}

  getAll(): Observable<Book[]> { return this.http.get<Book[]>(this.url); }
  getById(id: number): Observable<Book> { return this.http.get<Book>(`${this.url}/${id}`); }
  getAvailable(): Observable<Book[]> { return this.http.get<Book[]>(`${this.url}/available`); }
  create(dto: BookRequest): Observable<Book> { return this.http.post<Book>(this.url, dto); }
  update(id: number, dto: BookRequest): Observable<void> { return this.http.put<void>(`${this.url}/${id}`, dto); }
  delete(id: number): Observable<void> { return this.http.delete<void>(`${this.url}/${id}`); }
}
