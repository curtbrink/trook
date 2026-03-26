import type { DriverJob } from "@/api/models/driver-job.model.ts";
import type { Profile } from "@/api/models/profile.model.ts";
import type { PlayerJob } from "@/api/models/player-job.model.ts";
import type { Garage } from "@/api/models/garage.model.ts";
import type { LocalizedString } from "@/api/models/local-string.model.ts";

class ProfilesClient {
  private readonly baseEndpoint: string = "profiles";

  constructor(private client: TrookClient) {
  }

  async query(): Promise<Profile[]> {
    return await this.client.get<Profile[]>(this.baseEndpoint);
  }

  async create(name: string): Promise<Profile> {
    return await this.client.post<Profile>(this.baseEndpoint, { name });
  }

  async createFromFile(file: FormData): Promise<Profile> {
    return await this.client.post<Profile>(this.baseEndpoint, file);
  }
}

class StringsClient {
  private readonly baseEndpoint: string;

  constructor(private client: TrookClient, private readonly profileId: string) {
    this.baseEndpoint = `profiles/${profileId}/localization`;
  }

  async query(): Promise<LocalizedString[]> {
    return await this.client.get<LocalizedString[]>(this.baseEndpoint);
  }

  async create(key: string, value: string): Promise<LocalizedString> {
    return await this.client.post<LocalizedString>(this.baseEndpoint, { key, localized: value });
  }

  async update(stringId: string, value: string): Promise<LocalizedString> {
    const endpoint = `${this.baseEndpoint}/${stringId}`;
    return await this.client.put<LocalizedString>(endpoint, { localized: value });
  }
}

class FileIngestionClient {
  private readonly baseEndpoint: string;

  constructor(private client: TrookClient, private readonly profileId: string) {
    this.baseEndpoint = `profiles/${profileId}/files`;
  }

  async postFile(file: FormData): Promise<void> {
    return await this.client.post<void>(this.baseEndpoint, file);
  }
}

class GaragesClient {
  private readonly baseEndpoint: string;

  constructor(private client: TrookClient, private readonly profileId: string) {
    this.baseEndpoint = `profiles/${profileId}/garages`;
  }

  async query(): Promise<Garage[]> {
    return await this.client.get<Garage[]>(this.baseEndpoint);
  }
}

class DriverJobsClient {
  private readonly baseEndpoint: string;

  constructor(private client: TrookClient, private readonly profileId: string) {
    this.baseEndpoint = `profiles/${profileId}/jobs`;
  }

  async query(): Promise<DriverJob[]> {
    return await this.client.get<DriverJob[]>(this.baseEndpoint);
  }
}

class PlayerClient {
  private readonly baseEndpoint: string;

  constructor(private client: TrookClient, private readonly profileId: string) {
    this.baseEndpoint = `profiles/${profileId}/player`;
  }

  async query(): Promise<PlayerJob[]> {
    return await this.client.get<PlayerJob[]>(this.baseEndpoint);
  }
}

class TrookClient {
  constructor(private baseUrl: string) {
  }

  public async get<T>(url: string): Promise<T> {
    return await this.send<T>(url, "GET");
  }

  public async post<T>(url: string, body: FormData | any): Promise<T> {
    return await this.sendWithBody<T>(url, "POST", body);
  }

  public async put<T>(url: string, body: FormData | any): Promise<T> {
    return await this.sendWithBody<T>(url, "PUT", body);
  }

  private async sendWithBody<T>(url: string, method: string, body: FormData | any): Promise<T> {
    const isForm = body instanceof FormData;
    const reqBody = isForm ? body : JSON.stringify(body);
    const contentType = isForm ? "multipart/form-data" : "application/json";

    return await this.send<T>(url, method, reqBody, contentType);
  }

  private async send<T>(url: string, method: string, body: any = undefined, contentType: string = "application/json"): Promise<T> {
    const response = await fetch(`${this.baseUrl}/${url}`, {
      method,
      body,
      headers: !!body ? { "Content-Type": contentType } : undefined,
    });

    if (!response.ok)
      throw new Error(`API error: ${response.status}`);

    try {
      return await response.json();
    } catch (err) {
      return undefined as any;
    }
  }
}

const api = new TrookClient("/api/v1")
export const profilesApi = new ProfilesClient(api);
export const stringsApi = (profileId: string): StringsClient => new StringsClient(api, profileId);
export const filesApi = (profileId: string): FileIngestionClient => new FileIngestionClient(api, profileId);
export const garagesApi = (profileId: string): GaragesClient => new GaragesClient(api, profileId);
export const driverJobsApi = (profileId: string): DriverJobsClient => new DriverJobsClient(api, profileId);
export const playerJobsApi = (profileId: string): PlayerClient => new PlayerClient(api, profileId);
