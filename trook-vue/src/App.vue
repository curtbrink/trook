<template>
  <v-app id="inspire">
    <v-theme-provider theme="dark">
      <Suspense>
        <template #default>
          <AppContent class="fill-height" />
        </template>
        <template #fallback>
          <v-progress-circular indeterminate />
        </template>
      </Suspense>
      <v-snackbar-queue v-model="snackbar.messages" :total-visible="3" />
    </v-theme-provider>
  </v-app>
</template>

<script setup lang="ts">
import {useSnackbarStore} from "@/stores/snackbar.store.ts";
import AppContent from "@/views/AppContent.vue";

const snackbar = useSnackbarStore();
</script>
