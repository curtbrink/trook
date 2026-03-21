<template>
  <v-main fluid class="fill-height">
    <v-container fluid class="fill-height">
      <v-row>
        <v-col cols="2">
          <MenuList :links="links" />
        </v-col>

        <v-col cols="10">
          <v-sheet
            rounded="lg"
            height="100%"
          >
            <RouterView />
          </v-sheet>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script setup lang="ts">
import MenuList from "@/components/nav/MenuList.vue";

const links = [
  { text: 'Ingest File', route: 'file-ingestion' },
  { text: 'Edit Strings', route: 'file-ingestion' }, // todo
];
</script>
