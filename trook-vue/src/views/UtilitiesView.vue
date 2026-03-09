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
                    <v-btn class="w-100"
                           @click="selectFile">
                      Choose File...
                    </v-btn>
                    <input type="file" class="d-none" ref="fileInput" id="fileChooser" @change="setFile" />
                  </v-col>
                  <v-col cols="8">
                    <span class="d-flex align-center fill-height">Choose a file to ingest data from.</span>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="4">
                    <v-btn class="w-100">Clear Data</v-btn>
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
import {onMounted, ref, useTemplateRef} from "vue";
import type { VFileInput } from "vuetify/components/VFileInput";

const fileChooser = useTemplateRef('fileInput');

const selectFile = () => {
  console.log(fileChooser.value);
  fileChooser.value!.click();
};

const setFile = (e: any) => {
  console.log(e);
}
</script>
