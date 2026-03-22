<template>
  <v-sheet rounded="lg">
    <v-list rounded="lg">
      <v-list-item
        v-for="link in (props.links ?? [])"
        :key="link.route"
        :title="link.text"
        link
        :to="{ name: link.route }"
      ></v-list-item>

      <v-divider class="my-2"></v-divider>

      <v-list-item
        color="grey-lighten-4"
        title="Refresh"
        link
      ></v-list-item>
    </v-list>
  </v-sheet>
</template>

<script setup lang="ts">
export type MenuLink = {
  text: string;
  route: string;
};

const props = defineProps<{ links: MenuLink[] }>();
</script>

<style scoped>

</style>
