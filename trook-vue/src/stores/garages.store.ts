import { defineStore } from 'pinia'
import { garagesApi } from "@/api/client.ts";
import { useProfilesStore } from "@/stores/profiles.store.ts";
import type { Garage } from "@/api/models/garage.model.ts";

export const useGaragesStore = defineStore('garages', {
  state: () => ({
    garages: [] as Garage[],
    loading: false,
    loaded: false,
  }),
  actions: {
    async loadGarages() {
      if (this.loaded) return;

      this.loading = true;

      const profileId = useProfilesStore().selectedProfileId;
      if (!profileId) {
        this.loading = false;
        console.error("No profile selected");
        return;
      }

      try {
        this.garages = await garagesApi(profileId).query();
        this.loaded = true;
      } catch (err) {
        console.error("Failed to load garages", err);
      }
      this.loading = false;
    },
    async clear() {
      this.garages = [];
      this.loaded = false;
      this.loading = false;
    }
  }
})
