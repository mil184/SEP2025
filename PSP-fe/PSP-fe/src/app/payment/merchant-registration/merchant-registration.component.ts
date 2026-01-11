import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PaymentService } from '../payment.service';
import { Merchant } from '../../model/merchant.model';

@Component({
  selector: 'app-merchant-registration',
  templateUrl: './merchant-registration.component.html',
  styleUrl: './merchant-registration.component.css'
})
export class MerchantRegistrationComponent {
  merchantForm!: FormGroup;

  constructor(private service: PaymentService, private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.merchantForm = this.formBuilder.group({
      merchantName: [null, [Validators.required]],
      merchantPassword: [null, Validators.required],
      successUrl: [null, Validators.required],
      failedUrl: [null, Validators.required],
      errorUrl: [null, Validators.required],
    });
  }

  submit(): void {
  if (this.merchantForm.invalid) {
    this.merchantForm.markAllAsTouched();
    console.log('Form invalid', this.merchantForm.value);
    return;
  }

  const merchant: Merchant = {
    merchantName: this.merchantForm.value.merchantName || "",
    merchantPassword: this.merchantForm.value.merchantPassword || "", // FIXED (you had password1)
    successUrl: this.merchantForm.value.successUrl || "",
    failedUrl: this.merchantForm.value.failedUrl || "",
    errorUrl: this.merchantForm.value.errorUrl || "",
  };

  this.service.registerMerchant(merchant).subscribe({
    next: (res) => console.log('Merchant registered:', res),
    error: (err) => {
      console.error('Register merchant failed:', err);
      alert(err?.error?.title ?? err?.error?.message ?? 'Register merchant failed');
    }
  });
}

}
