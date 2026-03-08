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
            Response from backend but now within a routed view: {{ message }}
          </v-sheet>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script setup lang="ts">
import {ref, watchEffect} from "vue";
import {apiGet} from "@/api/client.ts";

const message = ref('');

watchEffect(async () => {
  const resp = await apiGet<{ foo: string }>('/api/v1/trook/hello');
  message.value = resp.foo;
})
</script>
