<template>
  <div>
  <v-app id="inspire">
    <v-theme-provider theme="dark">
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

      <RouterView />
<v-snackbar-queue v-model="snackbar.messages" :total-visible="3" />
    </v-theme-provider>
  </v-app>
  </div>
</template>

<script setup lang="ts">
import {useSnackbarStore} from "@/stores/snackbar.store.ts";

const links = [
  { title: 'Dashboard', route: '/' },
  { title: 'Utilities', route: '/utilities' },
]

const snackbar = useSnackbarStore();
</script>

<script lang="ts">
export default {
  data: () => ({
    links: [
      { title: 'Dashboard', route: '/' },
      { title: 'Utilities', route: '/utilities' },
    ],
  }),
}
</script>
