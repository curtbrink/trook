<template>
  <v-data-table
    :headers="columns"
    density="compact"
    :items="filteredItems"
  >
    <template v-slot:top>
      <v-checkbox v-model="showAllChecked" label="Show All" />
    </template>
    <template v-slot:item.city="{ item }">
      <LocalizedString :s="item.city" />
    </template>
  </v-data-table>
</template>

<script setup lang="ts">
import { onMounted, ref, watchEffect } from "vue";
import { useGaragesStore } from "@/stores/garages.store.ts";
import type { Garage } from "@/api/models/garage.model.ts";
import LocalizedString from "@/components/utilities/LocalizedString.vue";

const store = useGaragesStore();

const showAllChecked = ref<boolean>(false);
const filteredItems = ref<Garage[]>([]);

onMounted(() => {
  store.loadGarages();
});

watchEffect(() => {
  let items = store.garages;
  if (showAllChecked.value) {
    filteredItems.value = items;
    return;
  }
  // otherwise filter to owned
  filteredItems.value = items.filter((it => it.status != 0));
})

const columns = [
  { title: "City", key: "city" },
  { title: "Status", key: "status" },
  { title: "Productivity", key: "productivity" },
];
</script>

<style scoped>

</style>
