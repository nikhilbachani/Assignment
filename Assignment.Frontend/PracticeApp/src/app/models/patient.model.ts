export interface AddPatientRequest {
  firstName: string;
  lastName: string;
  dob: Date;
  email: string;
  phone: string;
  ssn: string;
}

export interface AddPatientResponse {
  patientId: number;
}

export interface Patient {
  patientId: number;
  patientName: string;
  dob: Date;
  email: string;
  phone: string;
}
