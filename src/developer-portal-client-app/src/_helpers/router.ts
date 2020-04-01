import Vue from 'vue'
import Router from 'vue-router'

import HomePage from '@/views/home-page.vue'
import InfoPage from '@/views/info-page.vue'
import DocumentationPage from '@/views/documentation-page.vue'
import OpenUserDataPage from '@/views/open-user-data-page.vue'
import IntegrateUserDataPage from '@/views/integrate-user-data-page.vue'
import CreatePage from '@/views/create-page.vue'
import ProjectPage from '@/views/project-page.vue'
import ProjectStartPage from '@/views/project-start-page.vue'
import ProjectsPage from '@/views/projects-page.vue'
// import AccountPage from '@/views/account-page.vue'
import TestApiPage from '@/views/apitest-page.vue'
import JsonSchemaPage from '@/views/json-schema-page.vue'
import PlatformApiRequestPage from '@/views/platform-api-request-page.vue'
import TestApplicationPage from '@/views/test-application-page.vue'
import { authGuard } from '../auth/auth-guard'

Vue.use(Router)

export const router = new Router({
  mode: 'history',
  routes: [
    { path: '/', component: HomePage },
    { path: '/documentation', component: DocumentationPage },
    { path: '/info', component: InfoPage },
    { path: '/share-user-data', component: OpenUserDataPage, beforeEnter: authGuard },
    { path: '/integrate-user-data', component: IntegrateUserDataPage, beforeEnter: authGuard },
    { path: '/create', component: CreatePage, beforeEnter: authGuard },
    { path: '/projects', component: ProjectsPage, beforeEnter: authGuard },
    { path: '/project', component: ProjectPage, beforeEnter: authGuard },
    { path: '/project-start', component: ProjectStartPage, beforeEnter: authGuard },
    { path: '/test-application', component: TestApplicationPage, beforeEnter: authGuard },
    // { path: '/account', component: AccountPage, beforeEnter: authGuard },
    { path: '/test-open-api', component: TestApiPage, beforeEnter: authGuard },
    { path: '/platform-api-requests', component: PlatformApiRequestPage, beforeEnter: authGuard },
    { path: '/json-schema', component: JsonSchemaPage, beforeEnter: authGuard },

    // otherwise redirect to home
    { path: '*', redirect: '/' }
  ],
  linkActiveClass: 'active', // active class for non-exact links.
  linkExactActiveClass: 'active' // active class for *exact* links.
})
