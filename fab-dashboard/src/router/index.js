import Vue from 'vue';
import VueRouter from 'vue-router';
import Dashboard from '../views/Dashboard.vue';
import Report from '../views/Report.vue';

Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    component: Dashboard,
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: Dashboard,
  },
  {
    path: '/report',
    name: 'Report',
    component: Report,
  }
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

export default router;
