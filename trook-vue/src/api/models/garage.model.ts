export interface Garage {
  id: string;
  createdAt: string;
  updatedAt: string;
  deletedAt?: string | null;

  profileId: string;

  city: string;
  status: number;
  productivity: number;
}
