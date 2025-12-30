import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Vehicle } from '../model/vehicle.model';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {

  private baseUrl = 'https://localhost:7133/vehicles';

  constructor(private http: HttpClient) { }

  getAll(): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(this.baseUrl);
  }

  getById(id: string): Observable<Vehicle> {
    return this.http.get<Vehicle>(`${this.baseUrl}/${id}`);
  }
}
