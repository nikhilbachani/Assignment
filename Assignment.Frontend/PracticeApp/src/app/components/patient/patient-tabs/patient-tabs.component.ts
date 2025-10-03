import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchPatientComponent } from '../search-patient/search-patient.component';
import { CreatePatientComponent } from '../create-patient/create-patient.component';

@Component({
  selector: 'app-patient',
  standalone: true,
  imports: [CommonModule, SearchPatientComponent, CreatePatientComponent],
  templateUrl: './patient-tabs.component.html',
  styleUrls: ['./patient-tabs.component.scss']
})
export class PatientComponent {
  activeTab: string = 'search';

  setActiveTab(tab: string) {
    this.activeTab = tab;
  }
}
