import { defineStore } from 'pinia'
import type { DriverJob } from "@/api/models/driver-job.model.ts";
import { driverJobsApi } from "@/api/client.ts";
import { useProfilesStore } from "@/stores/profiles.store.ts";

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

      const profileId = useProfilesStore().selectedProfileId;
      if (!profileId) {
        this.loading = false;
        console.error("No profile selected");
        return;
      }

      try {
        this.driverJobs = await driverJobsApi(profileId).query();
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
