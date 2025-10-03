import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AddPatientRequest } from '../../../models/patient.model';
import { PracticeApiService } from '../../../services/practice-api.service';
import { PatientDataService } from '../../../services/patient-data.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-create-patient',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-patient.component.html',
  styleUrls: ['./create-patient.component.scss'],
})
export class CreatePatientComponent {
  patientForm: FormGroup;
  isLoading: boolean = false;
  showError: boolean = false;
  createPatientSubscription$: Subscription | undefined;

  constructor(
    private fb: FormBuilder,
    private practiceApiService: PracticeApiService,
    private patientDataService: PatientDataService,
    private router: Router
  ) {
    this.patientForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      ssn: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d{3}[-\s]?\d{2}[-\s]?\d{4}$/),
        ],
      ],
      phone: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$/),
        ],
      ],
      dob: [
        '',
        [
          Validators.required,
        ],
      ],
    });
  }

  onSubmit() {
    if (this.patientForm.valid) {
      this.isLoading = true;

      const patient: AddPatientRequest = {
        firstName: this.patientForm.get('firstName')?.value,
        lastName: this.patientForm.get('lastName')?.value,
        email: this.patientForm.get('email')?.value,
        phone: this.patientForm.get('phone')?.value,
        dob: this.patientForm.get('dob')?.value,
        ssn: this.patientForm.get('ssn')?.value,
      };

      this.createPatientSubscription$ = this.practiceApiService.addPatient(patient).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.showError = false;

          this.patientDataService.setPatient({
            patientId: response.patientId,
            patientName: `${patient.firstName} ${patient.lastName}`,
            email: patient.email,
            phone: patient.phone,
            dob: patient.dob,
          });

          this.patientForm.reset();

          this.router.navigate(['/add-visit']);
        },
        error: () => {
          this.isLoading = false;
          this.showError = true;
        },
      });
    } else {
      this.patientForm.markAllAsTouched();
      this.showError = true;
    }
  }

  ngOnDestroy(): void {
    this.createPatientSubscription$?.unsubscribe();
  }
}
