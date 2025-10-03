import { Injectable } from '@angular/core';
import { Patient } from '../models/patient.model';

@Injectable({
  providedIn: 'root'
})
export class PatientDataService {
  private patient: Patient | undefined;

  setPatient(patient: Patient): void {
    this.patient = patient;
  }

  getPatient(): Patient | undefined {
    return this.patient;
  }

  clearPatient(): void {
    this.patient = undefined;
  }
}
