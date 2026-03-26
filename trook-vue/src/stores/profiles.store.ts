import { defineStore } from 'pinia'
import { profilesApi } from "@/api/client.ts";
import type { Profile } from "@/api/models/profile.model.ts";
import trookLocalStorage from "@/stores/local-storage.store.ts";
import { useDriverJobsStore } from "@/stores/driver-jobs.store.ts";

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
        this.profiles = await profilesApi.query();
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

      // clear data from other stores
      await useDriverJobsStore().clear();
    },
    async createProfile(name: string) {
      const createdProfile = await profilesApi.create(name);
      if (!createdProfile?.id) {
        console.error("Something went wrong");
        return;
      }

      this.profiles.push(createdProfile);
      await this.setSelectedProfileId(createdProfile.id);
    },
    async createProfileFromFile(form: FormData) {
      const createdProfile = await profilesApi.createFromFile(form);
      if (!createdProfile?.id) {
        console.error("Something went wrong");
        return;
      }

      this.profiles.push(createdProfile);
      await this.setSelectedProfileId(createdProfile.id);
    },
    async clear() {
      this.profiles = [];
      this.selectedProfileId = undefined;
      this.loaded = false;
      this.loading = false;
    }
  }
})
