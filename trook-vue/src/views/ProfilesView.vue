<template>
  <v-main>
    <v-container class="mx-auto d-flex align-center justify-center">
      <v-row>
        <v-col cols="2"/>
        <v-col cols="8">
          <v-card>
            <v-card-title>Create Profile</v-card-title>
            <v-card-text>
              <v-container fluid>
                <v-row>
                  <v-col cols="4">
                    <v-file-input label="Choose file" prepend-icon="mdi-paperclip" hide-details
                                  v-model="files" @change="filePicked"/>
                  </v-col>
                  <v-col cols="8">
                    <span
                      class="d-flex align-center fill-height">TODO: Choose a profile.sii to create a profile from</span>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="12">
                    <span class="d-flex align-center fill-height">Or, create one manually:</span>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="8">
                    <v-text-field v-model="inputProfileName" label="Profile name" placeholder="Super Awesome Trucking Ltd." />
                  </v-col>
                  <v-col cols="4">
                    <v-btn class="w-100" @click="submitProfileName">Create</v-btn>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>
          </v-card>

        </v-col>
        <v-col cols="2"/>
      </v-row>
    </v-container>
  </v-main>
</template>

<script setup lang="ts">
import { ref } from "vue";
import type { VFileInput } from "vuetify/components/VFileInput";
import { clearAllData, ingestFile } from "@/api/client.ts";
import { useSnackbarStore } from "@/stores/snackbar.store.ts";
import { useRouter } from "vue-router";
import { useDriverJobsStore } from "@/stores/driver-jobs.store.ts";
import { useProfilesStore } from "@/stores/profiles.store.ts";

const snackbar = useSnackbarStore();
const driverJobsStore = useDriverJobsStore();
const profileStore = useProfilesStore();
const router = useRouter();

const files = ref<File | null>(null);

const inputProfileName = ref<string | null>(null);

const filePicked = async () => {
  if (!files.value) return;
  const file = files.value;
  const formData = new FormData();
  formData.set("file", file);

  try {
    await ingestFile(formData);
    await snackbar.addMessage('Successfully uploaded file!');
    files.value = null;
    await router.push({ path: "/" });
  } catch (err) {
    console.error(err);
    await snackbar.addMessage(`Error occurred clearing data: ${err}`, true);
  }
}

const submitProfileName = async () => {
  if (!inputProfileName.value) return;

  try {
    await profileStore.createProfile(inputProfileName.value);
    await snackbar.addMessage("Successfully created profile! It's been selected for you!");
    await router.push({ path: "/" });
  } catch (err) {
    console.error(err);
    await snackbar.addMessage(`Error occurred creating profile: ${err}`, true);
  }
}
</script>
