import type {DriverJob} from "@/api/models/driver-job.model.ts";

export async function queryDriverJobs(): Promise<DriverJob[]> {
  const baseEndpoint = '/api/v1/jobs';

  // for now get all
  return apiGet<DriverJob[]>(baseEndpoint);
}

export async function clearAllData(): Promise<void> {
  const baseEndpoint = '/api/v1/admin/clear-all'
  return apiPost<void>(baseEndpoint, null);
}

export async function ingestFile(form: FormData): Promise<void> {
  const baseEndpoint = '/api/v1/admin/ingest-file';
  return apiPostForm<void>(baseEndpoint, form);
}

async function apiGet<T>(url: string): Promise<T> {
  const response = await fetch(url)

  if (!response.ok) {
    throw new Error(`API error: ${response.status}`)
  }

  return await response.json()
}

async function apiPostForm<T>(url: string, body: FormData): Promise<T> {
  const response = await fetch(url, {
    method: "POST",
    body
  });

  if (!response.ok) {
    throw new Error(`API error: ${response.status}`)
  }

  return await response.json()
}

async function apiPost<T>(url: string, body: any): Promise<T> {
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

  return await response.json()
}
