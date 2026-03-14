import { defineStore } from 'pinia'

export interface SnackbarItem {
  text: string,
  color: string,
  onDismiss: (reason: string) => void,
}

export const useSnackbarStore = defineStore('snackbar', {
  state: () => ({
    messages: [] as SnackbarItem[],
  }),
  actions: {
    async addMessage(message: string, isError: boolean = false) {
      this.messages.push({
        text: message,
        color: isError ? 'error' : 'success',
        onDismiss: (reason) => {
          console.log(`dismissed snack: ${reason}`);
        }
      });
    }
  }
});
