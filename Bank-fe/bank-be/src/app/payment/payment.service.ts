import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PayRequestDto {
  pan: string;
  securityCode: string;
  cardholderName: string;
  expirationDate: string; // "YYYY-MM-DD"
}

export interface PayResponseDto {
  redirectUrl: string;
}

@Injectable({ providedIn: 'root' })
export class PaymentService {
  constructor(private http: HttpClient) {}
  private readonly baseUrl = 'https://localhost:7147';

  pay(orderId: string, body: PayRequestDto): Observable<PayResponseDto> {
    return this.http.post<PayResponseDto>(`${this.baseUrl}/api/payments/pay/${encodeURIComponent(orderId)}`, body);
  }

  amount(orderId: string): Observable<number> {
    return this.http.get<number>(
      `${this.baseUrl}/api/payments/amount/${encodeURIComponent(orderId)}`
    );
  }

  qr(orderId: string): Observable<Blob> {
    return this.http.get(
      `${this.baseUrl}/api/payments/qr/${encodeURIComponent(orderId)}`,
      { responseType: 'blob' }
  );
}
}
