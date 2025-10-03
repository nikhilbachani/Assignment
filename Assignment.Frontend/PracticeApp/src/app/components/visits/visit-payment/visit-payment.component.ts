import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Subscription, timer } from 'rxjs';
import { VisitDataService } from '../../../services/visit-data.service';
import { VisitDetailsComponent } from "../visit-details/visit-details.component";
import { PracticeApiService } from '../../../services/practice-api.service';

@Component({
  selector: 'app-visit-payment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, VisitDetailsComponent],
  templateUrl: './visit-payment.component.html',
  styleUrls: ['./visit-payment.component.scss']
})
export class VisitPaymentComponent {
  visit: any;
  paymentForm: FormGroup;
  paymentSuccess: boolean | null = null;
  isProcessing: boolean = false;
  paymentMethodSubscription$: Subscription | undefined;
  payInvoiceSubscription$: Subscription | undefined;

  constructor(private fb: FormBuilder, private router: Router, private visitDataService: VisitDataService, private practiceApiService: PracticeApiService) {
    this.visit = this.visitDataService.getVisit();

    if (!this.visit) {
      this.router.navigate(['/']);
    }
    
    this.paymentForm = this.fb.group({
      paymentMethod: ['cash', Validators.required],
      cardNumber: [''],
      cardExpiry: [''],
      cardCVC: ['']
    });

    // Listen for changes in the payment method
    this.paymentMethodSubscription$ = this.paymentForm.get('paymentMethod')?.valueChanges.subscribe((method) => {
      if (method === 'card') {
        this.addCardValidators();
      } else {
        this.removeCardValidators();
      }
    });
  }

  addCardValidators() {
    this.paymentForm.get('cardNumber')?.setValidators([Validators.required, Validators.pattern(/\d{16}/)]);
    this.paymentForm.get('cardExpiry')?.setValidators([Validators.required, Validators.pattern(/^(0[1-9]|1[0-2])\/\d{2}$/)]);
    this.paymentForm.get('cardCVC')?.setValidators([Validators.required, Validators.pattern(/\d{3}/)]);
    this.paymentForm.get('cardNumber')?.updateValueAndValidity();
    this.paymentForm.get('cardExpiry')?.updateValueAndValidity();
    this.paymentForm.get('cardCVC')?.updateValueAndValidity();
  }

  removeCardValidators() {
    this.paymentForm.get('cardNumber')?.clearValidators();
    this.paymentForm.get('cardExpiry')?.clearValidators();
    this.paymentForm.get('cardCVC')?.clearValidators();
    this.paymentForm.get('cardNumber')?.updateValueAndValidity();
    this.paymentForm.get('cardExpiry')?.updateValueAndValidity();
    this.paymentForm.get('cardCVC')?.updateValueAndValidity();
  }

  onSubmit() {
    if (this.paymentForm.valid) {
      this.isProcessing = true;

      this.payInvoiceSubscription$ = this.practiceApiService.payInvoice(this.visit.invoiceId, this.paymentForm.value.paymentMethod).subscribe({
        next: () => {
          this.paymentSuccess = true;
          this.isProcessing = false;
          this.visitDataService.clearVisit();

          // Simulate processing delay
          timer(2000).subscribe(() => {
            this.router.navigate(['/']);
          });
        }
      });
    } else {
      this.paymentSuccess = false;
    }
  }

  ngOnDestroy(): void {
    this.paymentMethodSubscription$?.unsubscribe();
    this.payInvoiceSubscription$?.unsubscribe();
  }
}