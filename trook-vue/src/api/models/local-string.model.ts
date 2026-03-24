export interface LocalizedString {
  id: string;
  createdAt: string;
  updatedAt: string;
  deletedAt?: string | null;

  profileId: string;

  key: string;
  localized: string;
}
