<template>
  <v-main>
    <v-container class="mx-auto d-flex align-center justify-center">
      <v-row>
        <v-col cols="2" />
        <v-col cols="8">
          <v-card>
            <v-card-title>Utilities</v-card-title>
            <v-card-text>
              <v-container fluid>
                <v-row>
                  <v-col cols="4">
                    <v-file-input label="Choose file" prepend-icon="mdi-paperclip" hide-details
                                  v-model="files" @change="filePicked" />
                  </v-col>
                  <v-col cols="8">
                    <span class="d-flex align-center fill-height">Choose a file to ingest data from.</span>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="4">
                    <v-btn class="w-100" @click="clearData">Clear Data</v-btn>
                  </v-col>
                  <v-col cols="8">
                    <span class="d-flex fill-height align-center">warning: clears all data</span>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>
          </v-card>

        </v-col>
        <v-col cols="2" />
      </v-row>
    </v-container>
  </v-main>
</template>

<script setup lang="ts">
import {ref} from "vue";
import type { VFileInput } from "vuetify/components/VFileInput";
import {clearAllData, ingestFile} from "@/api/client.ts";
import {useSnackbarStore} from "@/stores/snackbar.store.ts";
import {useRouter} from "vue-router";
import {useDriverJobsStore} from "@/stores/driver-jobs.store.ts";

const snackbar = useSnackbarStore();
const driverJobsStore = useDriverJobsStore();
const router = useRouter();

const files = ref<File | null>(null);

const filePicked = async () => {
  if (!files.value) return;
  const file = files.value;
  const formData = new FormData();
  formData.set("file", file);

  try {
    await ingestFile(formData);
    await snackbar.addMessage('Successfully uploaded file!');
    files.value = null;
    await driverJobsStore.clear();
    await router.push({ path: "/" });
  } catch (err) {
    console.error(err);
    await snackbar.addMessage(`Error occurred clearing data: ${err}`, true);
  }
}

const clearData = async () => {
  try {
    await clearAllData();
    await driverJobsStore.clear();
    await snackbar.addMessage('Successfully cleared all data');
    await router.push({ path: "/" });
  } catch (err) {
    console.error(err);
    await snackbar.addMessage(`Error occurred clearing data: ${err}`, true);
  }
}
</script>
