import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api-service';
import { Publisher } from '../models/publisher.model';

@Injectable({ providedIn: 'root' })
export class PublisherService {
  private api = inject(ApiService);

  getAll(): Observable<Publisher[]> { return this.api.get<Publisher[]>('publisher'); }
}
