<template>
  <v-main>
    <v-container>
      <v-row>
        <v-col cols="2">
          <v-sheet rounded="lg">
            <v-list rounded="lg">
              <v-list-item
                v-for="n in 5"
                :key="n"
                :title="`List Item ${n}`"
                link
              ></v-list-item>

              <v-divider class="my-2"></v-divider>

              <v-list-item
                color="grey-lighten-4"
                title="Refresh"
                link
              ></v-list-item>
            </v-list>
          </v-sheet>
        </v-col>

        <v-col>
          <v-sheet
            min-height="70vh"
            rounded="lg"
          >
            Response from backend with a driver job: {{ message }}
          </v-sheet>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script setup lang="ts">
import {ref, watchEffect} from "vue";
import {apiGet} from "@/api/client.ts";
import type {DriverJob} from "@/api/models/driver-job.model.ts";

const message = ref('');

watchEffect(async () => {
  const resp = await apiGet<DriverJob[]>('/api/v1/trook/jobs');
  const first = resp[0]!;
  message.value = `${first.driverId} | ${first.sourceCity} => ${first.destinationCity} | ${first.distance}km`;
})
</script>
