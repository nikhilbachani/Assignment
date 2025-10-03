import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { VisitDataService } from '../../../services/visit-data.service';
import { Visit } from '../../../models/visit.model';

@Component({
  selector: 'app-visit-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './visit-details.component.html',
  styleUrls: ['./visit-details.component.scss']
})
export class VisitDetailsComponent {
  visit: Visit | undefined;
  @Input() providerView: boolean = false;

  constructor(private router: Router, private visitDataService: VisitDataService) { }

  ngOnInit() {
    this.visit = this.visitDataService.getVisit();

    if (!this.visit) {
      this.router.navigate(['/']);
    }
  }
}
