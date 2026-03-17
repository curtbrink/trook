import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import UtilitiesView from "@/views/UtilitiesView.vue";
import { useProfilesStore } from "@/stores/profiles.store.ts";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/utilities',
      name: 'utilities',
      component: UtilitiesView,
    }
  ],
});

router.beforeEach(async (to, from) => {
  const profileStore = useProfilesStore();

  if (!profileStore.loaded) {
    await profileStore.loadProfiles();
  }

  if (profileStore.profiles.length === 0) {
    if (to.name !== 'create-profile') {
      return { name: 'create-profile' };
    }
    return;
  }

  if (!profileStore.selectedProfileId) {
    // ultimate fallback if necessary
    await profileStore.setSelectedProfileId(profileStore.profiles[0]?.id);
  }
});

export default router;
