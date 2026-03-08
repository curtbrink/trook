export interface DriverJob {
  id: string; // guid
  createdAt: string; // datetime
  updatedAt: string; // datetime
  deletedAt?: string | null;

  driverId: string;

  dayCompleted: number;
  isEmpty: boolean;

  revenue: number;
  wage: number;
  maintenance: number;
  fuel: number;

  distance: number;

  cargoType?: string | null;
  cargoSize?: number | null;

  sourceCity: string;
  sourceCompany?: string | null;

  destinationCity: string;
  destinationCompany?: string | null;
}
