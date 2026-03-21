export interface PlayerJob {
  id: string;
  createdAt: string;
  updatedAt: string;
  deletedAt?: string | null;

  playerId: string;

  isQuickJob: boolean;

  startedAt: number;
  finishedAt: number;

  sourceCity: string;
  sourceCompany: string;
  destinationCity: string;
  destinationCompany: string;

  cargoType: string;
  cargoSize: number;
  cargoWeight: number;

  baseDistance: number;
  baseRevenue: number;
  distance: number;
  revenue: number;

  xp: number;
  parkingLevel: number;
}
