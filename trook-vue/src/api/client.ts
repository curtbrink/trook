import type {DriverJob} from "@/api/models/driver-job.model.ts";

export async function queryDriverJobs(): Promise<DriverJob[]> {
  const baseEndpoint = '/api/v1/trook/jobs';

  // for now get all
  return apiGet<DriverJob[]>(baseEndpoint);
}

async function apiGet<T>(url: string): Promise<T> {
  const response = await fetch(url)

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
