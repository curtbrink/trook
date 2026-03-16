import { defineStore } from 'pinia'
import {createProfile, queryDriverJobs, queryProfiles} from "@/api/client.ts";
import type {Profile} from "@/api/models/profile.model.ts";

export const useProfilesStore = defineStore('profiles', {
  state: () => ({
    profiles: [] as Profile[],
    loading: false,
    loaded: false,

    selectedProfile: undefined as (Profile | undefined),
  }),
  actions: {
    async loadProfiles() {
      if (this.loaded) return;

      this.loading = true;
      try {
        this.profiles = await queryProfiles();
        this.loaded = true;
        if (this.profiles.length === 0) {
          const profile = await createProfile("default");
          this.profiles.push(profile);
          this.selectedProfile = this.profiles[0];
        }
      } catch (err) {
        console.error("Failed to load profiles", err);
      }
      this.loading = false;
    },
    async clear() {
      this.profiles = [];
      this.selectedProfile = undefined;
      this.loaded = false;
      this.loading = false;
    }
  }
})
