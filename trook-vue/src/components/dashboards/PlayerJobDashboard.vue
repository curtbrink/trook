<template>
  <v-data-table
    :headers="columns"
    density="compact"
    :items="store.playerJobs">
    <!--suppress VueUnrecognizedSlot -->
    <template v-slot:item.isQuickJob="{ value }">
      <v-icon v-if="value" small>mdi-check</v-icon>
    </template>
  </v-data-table>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { usePlayerJobsStore } from "@/stores/player-jobs.store.ts";

const store = usePlayerJobsStore();

onMounted(() => {
  store.loadJobs();
});

const columns = [
  { title: "Quick?", key: "isQuickJob" },
  { title: "Start", key: "startedAt" },
  { title: "Finish", key: "finishedAt" },
  { title: "Cargo", key: "cargoType" },
  { title: "Amt", key: "cargoSize" },
  { title: "Wt", key: "cargoWeight" },
  { title: "Dist", key: "distance" },
  { title: "From City", key: "sourceCity" },
  { title: "From Co.", key: "sourceCompany" },
  { title: "To City", key: "destinationCity" },
  { title: "To Co.", key: "destinationCompany" },
  { title: "XP", key: "xp" },
  { title: "Profit", key: "revenue" },
];
</script>

<style scoped>

</style>
