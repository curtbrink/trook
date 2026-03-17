export interface Profile {
  id: string;
  createdAt: string;
  updatedAt: string;
  deletedAt?: string | null;

  name: string;
}
