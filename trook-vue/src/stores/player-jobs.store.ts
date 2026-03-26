import { defineStore } from 'pinia'
import { playerJobsApi } from "@/api/client.ts";
import { useProfilesStore } from "@/stores/profiles.store.ts";
import type { PlayerJob } from "@/api/models/player-job.model.ts";

export const usePlayerJobsStore = defineStore('player-jobs', {
  state: () => ({
    playerJobs: [] as PlayerJob[],
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
        this.playerJobs = await playerJobsApi(profileId).query();
        this.loaded = true;
      } catch (err) {
        console.error("Failed to load player jobs", err);
      }
      this.loading = false;
    },
    async clear() {
      this.playerJobs = [];
      this.loaded = false;
      this.loading = false;
    }
  }
})
