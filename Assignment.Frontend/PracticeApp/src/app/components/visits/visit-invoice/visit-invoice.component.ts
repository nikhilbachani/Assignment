import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { VisitDataService } from '../../../services/visit-data.service';

@Component({
  selector: 'app-visit-invoice',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './visit-invoice.component.html',
  styleUrls: ['./visit-invoice.component.scss']
})
export class VisitInvoiceComponent {
  visit: any;

  constructor(private router: Router, private visitDataService: VisitDataService) {
    this.visit = this.visitDataService.getVisit();

    if (!this.visit) {
      this.router.navigate(['/']);
    }
  }

  takePayment() {
    this.router.navigate(['/visit-payment']);
  }

  viewReceipt() {
    this.router.navigate(['/visit-receipt']);
  }
}