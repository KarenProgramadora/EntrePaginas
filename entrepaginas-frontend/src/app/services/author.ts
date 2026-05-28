import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Author, AuthorRequest } from '../models/models';

@Injectable({ providedIn: 'root' })
export class AuthorService {
  private url = `${environment.apiUrl}/author`;
  constructor(private http: HttpClient) {}

  getAll(): Observable<Author[]> { return this.http.get<Author[]>(this.url); }
  getById(id: number): Observable<Author> { return this.http.get<Author>(`${this.url}/${id}`); }
  create(dto: AuthorRequest): Observable<Author> { return this.http.post<Author>(this.url, dto); }
  update(id: number, dto: AuthorRequest): Observable<void> { return this.http.put<void>(`${this.url}/${id}`, dto); }
  delete(id: number): Observable<void> { return this.http.delete<void>(`${this.url}/${id}`); }
}
