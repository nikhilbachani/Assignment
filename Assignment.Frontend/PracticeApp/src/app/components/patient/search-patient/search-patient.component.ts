import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

import { debounceTime, distinctUntilChanged, switchMap, catchError, Observable, of, Subscription } from 'rxjs';

import { PracticeApiService } from '../../../services/practice-api.service';
import { PatientDataService } from '../../../services/patient-data.service';
import { Patient } from '../../../models/patient.model';

@Component({
  selector: 'app-search-patient',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './search-patient.component.html',
  styleUrls: ['./search-patient.component.scss']
})
export class SearchPatientComponent {
  searchForm: FormGroup;
  isLoading: boolean = false;
  filteredPatients$: Observable<Patient[]> = of([]);
  searchPatientSubscription$: Subscription | undefined;

  constructor(private fb: FormBuilder,
      private patientDataService: PatientDataService,
      private practiceApiService: PracticeApiService,
      private router: Router) {
    this.searchForm = this.fb.group({
      searchText: ['']
    });
  }

  onSearch() {
    const searchText = this.searchForm.get('searchText')?.value;

    this.isLoading = true; // Set loading state to true

    this.practiceApiService.searchPatients(searchText).pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((patients: Patient[]) => {
        this.isLoading = false;
        return of(patients);
      }),
      catchError((error) => {
        this.isLoading = false;
        console.error('Error fetching patients:', error);
        return of([]);
      })
    ).subscribe((patients: Patient[]) => {
      this.filteredPatients$ = of(patients); // Update the observable with the fetched patients
    });
}

  selectPatient(patient: Patient) {
    this.patientDataService.setPatient(patient);
    this.router.navigate(['/add-visit']);
  }

  ngOnDestroy(): void {
    this.searchPatientSubscription$?.unsubscribe();
  }
}
