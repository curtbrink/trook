import { defineStore } from 'pinia'
import type {DriverJob} from "@/api/models/driver-job.model.ts";
import {queryDriverJobs} from "@/api/client.ts";

export const useDriverJobsStore = defineStore('driver-jobs', {
  state: () => ({
    driverJobs: [] as DriverJob[],
    loading: false,
    loaded: false,
  }),
  actions: {
    async loadJobs() {
      if (this.loaded) return;

      this.loading = true;
      try {
        this.driverJobs = await queryDriverJobs();
        this.loaded = true;
      } catch (err) {
        console.error("Failed to load driver jobs", err);
      }
      this.loading = false;
    },
    async clear() {
      this.driverJobs = [];
      this.loaded = false;
      this.loading = false;
    }
  }
})
