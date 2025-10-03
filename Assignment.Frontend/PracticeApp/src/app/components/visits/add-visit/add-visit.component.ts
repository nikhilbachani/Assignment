import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { PatientDataService } from '../../../services/patient-data.service';
import { PracticeApiService } from '../../../services/practice-api.service';
import { Provider } from '../../../models/provider.model';
import { Observable, Subscription } from 'rxjs';
import { Patient } from '../../../models/patient.model';
import { AddVisitRequest } from '../../../models/visit.model';

@Component({
  selector: 'app-add-visit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-visit.component.html',
  styleUrls: ['./add-visit.component.scss']
})
export class AddVisitComponent {
  visitForm: FormGroup;
  selectedPatient: Patient | undefined;
  visitBookingError: boolean = false;
  availableTimeSlots: string[] = [];
  providers$: Observable<Provider[]>;
  addVisitSubscription$: Subscription | undefined;

  constructor(private fb: FormBuilder, private router: Router, private patientDataService: PatientDataService, private practiceApiService: PracticeApiService) {
    this.providers$ = this.practiceApiService.getProviders();

    this.visitForm = this.fb.group({
      provider: ['', Validators.required],
      time: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.selectedPatient = this.patientDataService.getPatient();

    if (!this.selectedPatient) {
      console.error('No patient data found. Please select/create a patient first.');
      this.router.navigate(['/patient']);
    }
  }

  onSubmit() {
    if (this.visitForm.valid && this.selectedPatient) {
      const visitData = {
        patientId: this.selectedPatient.patientId,
        providerId: this.visitForm.value.provider,
        visitDate: new Date().toISOString().split('T')[0],
        visitTime: this.visitForm.value.time
      } as AddVisitRequest;

      this.addVisitSubscription$ = this.practiceApiService.addVisit(visitData).subscribe({
        next: () => {
          this.visitForm.reset();
          this.router.navigate(['/']); // Navigate back to home
          this.visitBookingError = false;
          this.patientDataService.clearPatient(); // Clear selected patient after booking
        },
        error: () => {
          this.visitBookingError = true;
        }
      });
    } else {
      this.visitForm.markAllAsTouched();
      this.visitBookingError = true;
    }
  }

  onProviderChange(providerId: string) {
    if (providerId) {
      this.practiceApiService.getAvailableTimeSlots(+providerId, new Date().toISOString().split('T')[0]).subscribe({
        next: (timeSlots) => {
          this.availableTimeSlots = timeSlots;
        },
        error: (err) => {
          console.error('Error fetching time slots:', err);
          this.availableTimeSlots = [];
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.addVisitSubscription$?.unsubscribe();
  }
}