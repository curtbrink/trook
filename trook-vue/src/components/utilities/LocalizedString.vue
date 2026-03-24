<script setup lang="ts">
import { useLocalization } from "@/stores/local-strings.store.ts";
import { computed } from "vue";

const localizationStore = useLocalization();
const props = defineProps<{ s: string }>();
const theString = computed(() => localizationStore.getLocalized(props.s));
</script>

<template>
  <span>{{ theString }}</span>
</template>

<style scoped>

</style>
