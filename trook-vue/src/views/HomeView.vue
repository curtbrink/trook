<template>
  <v-main fluid class="fill-height">
    <v-container fluid class="fill-height">
      <v-row>
        <v-col cols="2">
          <v-sheet rounded="lg">
            <v-list rounded="lg">
              <v-list-item
                v-for="link in links"
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
const links = [
  { text: 'Driver Jobs', route: 'driver-job-dashboard' },
  { text: 'Player Jobs', route: 'player-job-dashboard' },
];
</script>
