import type { DriverJob } from "@/api/models/driver-job.model.ts";
import type { Profile } from "@/api/models/profile.model.ts";

export async function queryDriverJobs(profileId: string): Promise<DriverJob[]> {
  const baseEndpoint = `/api/v1/profiles/${profileId}/jobs`;
  return apiGet<DriverJob[]>(baseEndpoint);
}

export async function queryProfiles(): Promise<Profile[]> {
  const baseEndpoint = '/api/v1/profiles';
  return apiGet<Profile[]>(baseEndpoint);
}

export async function createProfile(name: string): Promise<Profile> {
  const baseEndpoint = '/api/v1/profiles';
  return apiPost<Profile>(baseEndpoint, { name });
}

export async function createProfileFromFile(form: FormData): Promise<Profile> {
  const baseEndpoint = '/api/v1/profiles';
  return apiPostForm<Profile>(baseEndpoint, form);
}

export async function clearAllData(): Promise<void> {
  const baseEndpoint = '/api/v1/admin/clear-all'
  return apiPost<void>(baseEndpoint, null);
}

export async function ingestDriverJobs(profileId: string, form: FormData): Promise<DriverJob[]> {
  const baseEndpoint = `/api/v1/profiles/${profileId}/jobs`;
  return apiPostForm<DriverJob[]>(baseEndpoint, form);
}

async function apiGet<T = void>(url: string): Promise<T extends void ? void : T> {
  const response = await fetch(url)

  if (!response.ok) {
    throw new Error(`API error: ${response.status}`)
  }

  return await parseJson<T>(response);
}

async function apiPostForm<T = void>(url: string, body: FormData): Promise<T extends void ? void : T> {
  const response = await fetch(url, {
    method: "POST",
    body
  });

  if (!response.ok) {
    throw new Error(`API error: ${response.status}`)
  }

  return await parseJson<T>(response);
}

async function apiPost<T = void>(url: string, body: any): Promise<T extends void ? void : T> {
  const response = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(body)
  })

  if (!response.ok) {
    throw new Error(`API error: ${response.status}`)
  }

  return await parseJson<T>(response);
}

async function parseJson<T = void>(response: Response): Promise<T extends void ? void : T> {
  try {
    return await response.json();
  } catch (err) {
    return undefined as any;
  }
}
