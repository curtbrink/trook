<template>
  <v-main>
    <v-container fluid>
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

        <v-col cols="10">
          <v-sheet
            min-height="70vh"
            rounded="lg"
          >
            <v-data-table
              :headers="columns"
              :items="store.driverJobs" />
          </v-sheet>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script setup lang="ts">
import {onMounted, ref} from "vue";
import {useDriverJobsStore} from "@/stores/driver-jobs.store.ts";
import type {DriverJob} from "@/api/models/driver-job.model.ts";

const store = useDriverJobsStore();

onMounted(() => {
  store.loadJobs();
});

const columns = [
  { title: "Day", key: "dayCompleted" },
  { title: "Driver", key: "driverId" },
  { title: "Cargo", key: "cargoType" },
  { title: "Amt", key: "cargoSize" },
  { title: "Dist", key: "distance" },
  { title: "From City", key: "sourceCity" },
  { title: "From Co.", key: "sourceCompany" },
  { title: "To City", key: "destinationCity" },
  { title: "To Co.", key: "destinationCompany" },
  {
    title: "Profit",
    key: "profit",
    value: (item: DriverJob) => item.revenue - item.wage - item.fuel - item.maintenance,
  },
];
</script>
