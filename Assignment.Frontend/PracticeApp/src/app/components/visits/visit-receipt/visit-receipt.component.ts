import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Visit } from '../../../models/visit.model';
import { VisitDataService } from '../../../services/visit-data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-visit-receipt',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './visit-receipt.component.html',
  styleUrls: ['./visit-receipt.component.scss']
})
export class VisitReceiptComponent {
  visit: Visit | undefined;
  constructor(private visitDataService: VisitDataService, private router: Router) { }
  ngOnInit(): void {
    this.visit = this.visitDataService.getVisit();

    if (!this.visit) {
      this.router.navigate(['/']);
    }
  }

}