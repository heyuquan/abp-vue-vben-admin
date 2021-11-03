import type { AppRouteModule } from '/@/router/types';

import { LAYOUT } from '/@/router/constant';
//import { t } from '/@/hooks/web/useI18n';

const saas: AppRouteModule = {
  path: '/saas',
  name: 'saas',
  component: LAYOUT,
  redirect: '/saas/tenants',
  meta: {
    orderNo: 10,
    icon: 'ion:grid-outline',
    title: 'Saas',
  },
  children: [
    {
      path: 'tenants',
      name: 'tenants',
      component: () => import('/@/views/saas/tenant/index.vue'),
      meta: {
        // affix: true,
        title: '租户',
      },
    },
    {
      path: 'editions',
      name: 'editions',
      component: () => import('/@/views/saas/edition/index.vue'),
      meta: {
        // affix: true,
        title: 'title-editions',
      },
    },
  ],
};

export default saas;
