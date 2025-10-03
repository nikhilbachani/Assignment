export interface Visit {
  visitId: number;
  visitDate: Date;
  visitTime: string;
  providerName: string;
  patientName: string;
  notes: string;
  visitAmount?: number;
  invoiceId?: number;
  paymentStatus?: string;
  paymentMethod?: string;
  receiptIdentifier?: string;
  paymentDate?: Date;
}

export interface AddVisitRequest {
  visitDate: string;
  visitTime: string;
  providerId: number;
  patientId: number;
  notes: string;
}

export interface AddVisitResponse {
  visitId: number;
}
