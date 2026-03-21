<template>
  <v-data-table
    :headers="columns"
    density="compact"
    :items="store.driverJobs"/>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useDriverJobsStore } from "@/stores/driver-jobs.store.ts";
import type { DriverJob } from "@/api/models/driver-job.model.ts";

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

<style scoped>

</style>
