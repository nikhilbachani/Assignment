import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { VisitDataService } from '../../../services/visit-data.service';
import { Visit } from '../../../models/visit.model';

@Component({
  selector: 'app-visits-table',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './visits-table.component.html',
  styleUrls: ['./visits-table.component.scss']
})
export class VisitsTableComponent {
  @Input() visits: Visit[] = [];
  @Input() providerView: boolean = false;

  constructor(private visitDataService: VisitDataService) { }

  setVisitData(visit: Visit) {
    this.visitDataService.setVisit(visit);
  }
}
