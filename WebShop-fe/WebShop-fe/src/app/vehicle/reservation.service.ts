import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Reservation } from '../model/reservation.model';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {

  private baseUrl = 'https://localhost:7133/reservations';
  
  constructor(private http: HttpClient) { }
  
  create(request: Reservation): Observable<any> {
    return this.http.post<any>(this.baseUrl, request);
  }
  
}
