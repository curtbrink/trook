import { defineStore } from 'pinia'
import {queryProfiles} from "@/api/client.ts";
import type {Profile} from "@/api/models/profile.model.ts";
import trookLocalStorage from "@/stores/local-storage.store.ts";

export const useProfilesStore = defineStore('profiles', {
  state: () => ({
    profiles: [] as Profile[],
    loading: false,
    loaded: false,

    selectedProfileId: undefined as string | undefined,
  }),
  getters: {
    selectedProfile: (state) =>
      state.profiles.find(p => p.id === state.selectedProfileId),
  },
  actions: {
    async loadProfiles() {
      if (this.loaded) return;

      this.loading = true;
      try {
        this.profiles = await queryProfiles();
      } catch (err) {
        console.error("Failed to load profiles", err);
        this.loading = false;
        return;
      }

      if (this.profiles.length === 0) {
        this.loaded = true;
        this.loading = false;
        return;
      }

      // get last-selected profile out of local storage
      const lastProfileId = trookLocalStorage.get("selectedProfileId");
      const lastProfileExists = this.profiles.find(p => p.id === lastProfileId);
      if (lastProfileId && lastProfileExists) {
        await this.setSelectedProfileId(lastProfileId);
      } else {
        await this.setSelectedProfileId(this.profiles[0]?.id);
      }

      this.loaded = true;
      this.loading = false;
    },
    async setSelectedProfileId(profileId?: string) {
      this.selectedProfileId = profileId;
      trookLocalStorage.set("selectedProfileId", profileId);
    },
    async clear() {
      this.profiles = [];
      this.selectedProfileId = undefined;
      this.loaded = false;
      this.loading = false;
    }
  }
})
