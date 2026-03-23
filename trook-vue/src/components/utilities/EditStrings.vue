<template>
  <div>
    <v-data-table
      :headers="headers"
      :hide-default-footer="localizationStore.localStrings.length < 11"
      :items="localizationStore.localStrings"
    >
      <template v-slot:top>
        <v-toolbar flat>
          <v-toolbar-title>
            <v-icon color="medium-emphasis" icon="mdi-book-multiple" size="x-small" start></v-icon>
            Pretty Strings
          </v-toolbar-title>

          <v-btn
            class="me-2"
            prepend-icon="mdi-plus"
            rounded="lg"
            text="Add a string"
            border
            @click="add"
          ></v-btn>
        </v-toolbar>
      </template>

      <!--suppress VueUnrecognizedSlot -->
      <template v-slot:item.title="{ value }">
        <v-chip :text="value" border="thin opacity-25" prepend-icon="mdi-book" label>
          <template v-slot:prepend>
            <v-icon color="medium-emphasis"></v-icon>
          </template>
        </v-chip>
      </template>

      <!--suppress VueUnrecognizedSlot -->
      <template v-slot:item.actions="{ item }">
        <div class="d-flex ga-2 justify-end">
          <v-icon color="medium-emphasis" icon="mdi-pencil" size="small"
                  @click="edit(item.id)"></v-icon>
        </div>
      </template>

      <template v-slot:no-data>
        <v-btn
          prepend-icon="mdi-backup-restore"
          rounded="lg"
          text="Reset data"
          variant="text"
          border
          @click="reset"
        ></v-btn>
      </template>
    </v-data-table>
    <v-dialog v-model="dialog" max-width="500">
      <v-card
        :subtitle="`${isEditing ? 'Update' : 'Create'} a pretty string`"
        :title="`${isEditing ? 'Edit' : 'Add'} a pretty string`"
      >
        <template v-slot:text>
          <v-row>
            <v-col cols="6">
              <v-text-field v-model="formModel.key" :disabled="isEditing" label="Key"></v-text-field>
            </v-col>

            <v-col cols="6">
              <v-text-field v-model="formModel.pretty" label="Text to display"></v-text-field>
            </v-col>
          </v-row>
        </template>

        <v-divider></v-divider>

        <v-card-actions class="bg-surface-light">
          <v-btn text="Cancel" variant="plain" @click="dialog = false"></v-btn>

          <v-spacer></v-spacer>

          <v-btn text="Save" @click="save"></v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref, shallowRef, toRef } from 'vue'
import { useLocalization } from "@/stores/local-strings.store.ts";

type StringRecord = { id?: string; key: string; pretty: string };

const localizationStore = useLocalization();

function createNewRecord(): StringRecord {
  return {
    key: '',
    pretty: '',
  } as StringRecord;
}

const formModel = ref<StringRecord>(createNewRecord())
const dialog = shallowRef(false)
const isEditing = toRef(() => !!formModel.value.id)

const headers = [
  { title: 'Key', key: 'key' },
  { title: 'Pretty', key: 'localized' },
  { title: 'Actions', key: 'actions', sortable: false },
];

onMounted(() => {
  reset();
});

function add() {
  formModel.value = createNewRecord();
  dialog.value = true;
}

function edit(id: string) {
  const found = localizationStore.localStrings.find(record => record.id === id);
  if (!found) return;

  formModel.value = {
    id: found.id,
    key: found.key,
    pretty: found.localized,
  };

  dialog.value = true;
}

async function save() {
  if (isEditing.value && formModel.value.id) {
    await localizationStore.updateString(formModel.value.id, formModel.value.pretty);
  } else {
    await localizationStore.addString(formModel.value.key, formModel.value.pretty);
  }

  dialog.value = false;
}

function reset() {
  dialog.value = false;
  formModel.value = createNewRecord();
}

</script>

<style scoped>

</style>
