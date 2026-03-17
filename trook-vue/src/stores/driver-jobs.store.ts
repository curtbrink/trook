import { defineStore } from 'pinia'
import type { DriverJob } from "@/api/models/driver-job.model.ts";
import { ingestDriverJobs, queryDriverJobs } from "@/api/client.ts";
import { useProfilesStore } from "@/stores/profiles.store.ts";
import { useSnackbarStore } from "@/stores/snackbar.store.ts";

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
        this.driverJobs = await queryDriverJobs(profileId);
        this.loaded = true;
      } catch (err) {
        console.error("Failed to load driver jobs", err);
      }
      this.loading = false;
    },
    async postJobs(form: FormData) {
      const profileId = useProfilesStore().selectedProfileId;
      if (!profileId) {
        console.error("No profile selected");
        return;
      }

      try {
        const jobsSaved = await ingestDriverJobs(profileId, form);
        this.driverJobs.push(...jobsSaved);
      } catch (err) {
        console.error("Failed to save driver jobs from file", err);
      }
    },
    async clear() {
      this.driverJobs = [];
      this.loaded = false;
      this.loading = false;
    }
  }
})
