<template>
  <v-main>
    <v-container class="mx-auto align-center justify-center">
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
      <v-row v-if="profileStore.profiles.length">
        <v-col cols="4"/>
        <v-col cols="4">
          <v-card>
            <v-card-title>Select Profile</v-card-title>
            <v-card-text>
              <v-data-table hide-default-footer :headers="profileHeaders" :items="profileStore.profiles">
                <!--suppress VueUnrecognizedSlot -->
                <template v-slot:item.select="{ item }">
                  <v-btn v-if="item.id !== profileStore.selectedProfileId" @click="selectProfile(item)">Select</v-btn>
                  <v-chip v-else>Selected!</v-chip>
                </template>
              </v-data-table>
            </v-card-text>
          </v-card>
        </v-col>
        <v-col cols="4"/>
      </v-row>
    </v-container>
  </v-main>
</template>

<script setup lang="ts">
import { ref } from "vue";
import type { VFileInput } from "vuetify/components/VFileInput";
import { useSnackbarStore } from "@/stores/snackbar.store.ts";
import { useRouter } from "vue-router";
import { useProfilesStore } from "@/stores/profiles.store.ts";
import type { Profile } from "@/api/models/profile.model.ts";

const snackbar = useSnackbarStore();
const profileStore = useProfilesStore();
const router = useRouter();

const profileHeaders = [
  { title: "", key: "select", width: "20%", align: "center" },
  { title: "Name", key: "name", width: "50%", align: "center" },
  { title: "Created", key: "createdAt", width: "30%", align: "center" },
];

const files = ref<File | null>(null);

const inputProfileName = ref<string | null>(null);

const selectProfile = async (profile: Profile) => {
  if (!profile.id) return;

  await profileStore.setSelectedProfileId(profile.id);
  await snackbar.addMessage(`Selected profile "${profile.name}"!`);
  await router.push("/");
}

const filePicked = async () => {
  if (!files.value) return;
  const file = files.value;
  const formData = new FormData();
  formData.set("file", file);

  try {
    await profileStore.createProfileFromFile(formData);
    await snackbar.addMessage("Successfully created profile! It's been selected for you!");
    files.value = null;
    await router.push({ path: "/" });
  } catch (err) {
    console.error(err);
    await snackbar.addMessage(`Error occurred creating profile: ${err}`, true);
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
