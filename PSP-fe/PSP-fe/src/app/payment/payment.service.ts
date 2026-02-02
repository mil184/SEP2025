import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Merchant } from '../model/merchant.model';

export interface BankPaymentResponse {
  paymentUrl: string;
  paymentId: string;
}

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private readonly baseUrl = 'https://localhost:7154';

  constructor(private http: HttpClient) {}

  createBankPaymentRequest(orderId: string): Observable<BankPaymentResponse> {
    return this.http.post<BankPaymentResponse>(
      `${this.baseUrl}/api/bank-payment-requests/${encodeURIComponent(orderId)}`,
      {}
    );
  }

  createBankPaymentQRRequest(orderId: string): Observable<BankPaymentResponse> {
    return this.http.post<BankPaymentResponse>(
      `${this.baseUrl}/api/bank-payment-requests/qr/${encodeURIComponent(orderId)}`,
      {}
    );
  }

  registerMerchant(merchant: Merchant): Observable<Merchant> {
    return this.http.post<Merchant>(`${this.baseUrl}/api/payments/merchant`, merchant);
  }
}