import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Publisher } from '../models/models';

@Injectable({ providedIn: 'root' })
export class PublisherService {
  private url = `${environment.apiUrl}/publisher`;
  constructor(private http: HttpClient) {}

  getAll(): Observable<Publisher[]> { return this.http.get<Publisher[]>(this.url); }
}
