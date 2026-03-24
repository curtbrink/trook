import { defineStore } from 'pinia'
import { ingestDataFromFile } from "@/api/client.ts";
import { useProfilesStore } from "@/stores/profiles.store.ts";
import { useDriverJobsStore } from "@/stores/driver-jobs.store.ts";
import { usePlayerJobsStore } from "@/stores/player-jobs.store.ts";
import { useGaragesStore } from "@/stores/garages.store.ts";

export const useFileIngestionStore = defineStore('file-ingestion', {
  state: () => ({}),
  actions: {
    async postJobs(form: FormData) {
      const profileStore = useProfilesStore();
      const driverJobStore = useDriverJobsStore();
      const playerJobStore = usePlayerJobsStore();
      const garageStore = useGaragesStore();
      // TODO other data types extracted from files' stores here

      const profileId = profileStore.selectedProfileId;
      if (!profileId) {
        console.error("No profile selected");
        return;
      }

      try {
        await ingestDataFromFile(profileId, form);
        await driverJobStore.clear();
        await playerJobStore.clear();
        await garageStore.clear();
      } catch (err) {
        console.error("Failed to save driver jobs from file", err);
      }
    }
  }
})
