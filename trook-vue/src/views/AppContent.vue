<template>
  <div>
    <v-app-bar flat>
      <v-container fluid class="mx-auto d-flex align-center justify-center">
        <v-avatar
          class="me-4 "
          color="grey-darken-1"
          size="32"
        ></v-avatar>

        <v-btn
          v-for="link in links"
          :key="link.title"
          :text="link.title"
          :to="link.route"
          variant="text"
        ></v-btn>

        <v-spacer></v-spacer>

        <v-responsive max-width="160">
          <v-text-field
            density="compact"
            label="Search"
            rounded="lg"
            variant="solo-filled"
            flat
            hide-details
            single-line
          ></v-text-field>
        </v-responsive>
      </v-container>
    </v-app-bar>

    <RouterView/>
  </div>

</template>

<script setup lang="ts">
import { useProfilesStore } from "@/stores/profiles.store.ts";

const links = [
  { title: 'Dashboard', route: '/' },
  { title: 'Utilities', route: '/utilities' },
]

const profileStore = useProfilesStore();

await profileStore.loadProfiles();
</script>
