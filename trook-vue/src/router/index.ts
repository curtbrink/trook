import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import UtilitiesView from "@/views/UtilitiesView.vue";
import { useProfilesStore } from "@/stores/profiles.store.ts";
import ProfilesView from "@/views/ProfilesView.vue";
import DriverJobDashboard from "@/components/dashboards/DriverJobDashboard.vue";
import PlayerJobDashboard from "@/components/dashboards/PlayerJobDashboard.vue";
import FileIngestion from "@/components/utilities/FileIngestion.vue";
import GarageDashboard from "@/components/dashboards/GarageDashboard.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      redirect: { name: 'driver-job-dashboard' },
      component: HomeView,
      children: [
        {
          path: 'driver-jobs',
          name: 'driver-job-dashboard',
          component: DriverJobDashboard,
        },
        {
          path: 'player-jobs',
          name: 'player-job-dashboard',
          component: PlayerJobDashboard,
        },
        {
          path: 'garages',
          name: 'garage-dashboard',
          component: GarageDashboard,
        }
      ]
    },
    {
      path: '/utilities',
      name: 'utilities',
      redirect: { name: 'file-ingestion' },
      component: UtilitiesView,
      children: [
        {
          path: 'file-ingestion',
          name: 'file-ingestion',
          component: FileIngestion
        }
      ]
    },
    {
      path: '/profiles/create',
      name: 'create-profile',
      component: ProfilesView,
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
