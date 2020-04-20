import Vue from 'vue'
import Router from 'vue-router'

import HomePage from '@/views/home-page.vue'
import InfoPage from '@/views/info-page.vue'
import DocumentationPage from '@/views/documentation-page.vue'
import ApplicationSettingsPage from '@/views/application-settings-page.vue'
import ApplicationDocumentationPage from '@/views/application-documentation-page.vue'
import CreatePage from '@/views/create-page.vue'
import ProjectPage from '@/views/project-page.vue'
import ProjectStartPage from '@/views/project-start-page.vue'
import ProjectsPage from '@/views/projects-page.vue'
import PlatformSettingsPage from '@/views/platform-settings-page.vue'
import PlatformTestPage from '@/views/platform-test-page.vue'
import JsonSchemaPage from '@/views/json-schema-page.vue'
import PlatformDocumentationPage from '@/views/platform-documentation-page.vue'
import ApplicationTestPage from '@/views/application-test-page.vue'
import { authGuard } from '../auth/auth-guard'

Vue.use(Router)

export const router = new Router({
  mode: 'history',
  routes: [
    { path: '/', component: HomePage },
    { path: '/documentation', component: DocumentationPage },
    { path: '/info', component: InfoPage },
    { path: '/application-settings', component: ApplicationSettingsPage, beforeEnter: authGuard },
    { path: '/application-test', component: ApplicationTestPage, beforeEnter: authGuard },
    { path: '/application-documentation', component: ApplicationDocumentationPage, beforeEnter: authGuard },
    { path: '/create', component: CreatePage, beforeEnter: authGuard },
    { path: '/projects', component: ProjectsPage, beforeEnter: authGuard },
    { path: '/project', component: ProjectPage, beforeEnter: authGuard },
    { path: '/project-start', component: ProjectStartPage, beforeEnter: authGuard },
    { path: '/platform-settings', component: PlatformSettingsPage, beforeEnter: authGuard },
    { path: '/platform-test', component: PlatformTestPage, beforeEnter: authGuard },
    { path: '/platform-documentation', component: PlatformDocumentationPage, beforeEnter: authGuard },
    { path: '/json-schema', component: JsonSchemaPage, beforeEnter: authGuard },

    // otherwise redirect to home
    { path: '*', redirect: '/' }
  ],
  linkActiveClass: 'active', // active class for non-exact links.
  linkExactActiveClass: 'active' // active class for *exact* links.
})
