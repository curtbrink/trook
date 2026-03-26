import { defineStore } from 'pinia'
import { stringsApi } from "@/api/client.ts";
import { useProfilesStore } from "@/stores/profiles.store.ts";
import type { LocalizedString } from "@/api/models/local-string.model.ts";

export const useLocalization = defineStore('localization', {
  state: () => ({
    localStrings: [] as LocalizedString[],
    loading: false,
    loaded: false,
  }),
  actions: {
    async loadStrings() {
      if (this.loaded) return;

      this.loading = true;

      const profileId = useProfilesStore().selectedProfileId;
      if (!profileId) {
        this.loading = false;
        console.error("No profile selected");
        return;
      }

      try {
        this.localStrings = await stringsApi(profileId).query();
        this.loaded = true;
      } catch (err) {
        console.error("Failed to load localized strings", err);
      }
      this.loading = false;
    },
    async clear() {
      this.localStrings = [];
      this.loaded = false;
      this.loading = false;
    },
    getLocalized(s: string) {
      const entry = this.localStrings.find(it => it.key == s);
      if (entry)
        return entry.localized;
      return s;
    },
    async updateString(id: string, value: string) {
      const profileId = useProfilesStore().selectedProfileId;
      if (!profileId) {
        this.loading = false;
        console.error("No profile selected");
        return;
      }

      const idx = this.localStrings.findIndex(it => it.id == id);
      if (idx == -1) return;

      const newEntry = await stringsApi(profileId).update(id, value);
      this.localStrings.splice(idx, 1, newEntry);
    },
    async addString(key: string, value: string) {
      const profileId = useProfilesStore().selectedProfileId;
      if (!profileId) {
        this.loading = false;
        console.error("No profile selected");
        return;
      }

      const newEntry = await stringsApi(profileId).create(key, value);
      this.localStrings.push(newEntry);
    }
  }
})
