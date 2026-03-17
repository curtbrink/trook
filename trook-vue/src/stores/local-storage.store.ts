// not really a pinia store but a wrapper for local storage because localhost
const localStoragePrefix = "trook";
const getKey = (key: string) => `${localStoragePrefix}.${key}`;

export default {
  get(key: string): string | null {
    return localStorage.getItem(getKey(key));
  },
  set(key: string, value?: string): void {
    localStorage.setItem(getKey(key), value ?? "");
  }
}
