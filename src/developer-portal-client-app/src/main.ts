import Vue from 'vue'
import App from './App.vue'

import {router} from './_helpers/router'
import store from './store/index'

import { ValidationProvider, extend } from 'vee-validate'
import { required, min, max, email, numeric, confirmed, regex } from 'vee-validate/dist/rules'

import { domain, clientId, audience } from '../auth_config.json'
import { Auth0Plugin } from './auth'

// Import loading component
import Loading from 'vue-loading-overlay'
import 'vue-loading-overlay/dist/vue-loading.css'

// Import modal
import VModal from 'vue-js-modal'

Vue.use(VModal)

Vue.use(Auth0Plugin, {
  domain,
  clientId,
  audience,
  onRedirectCallback: appState => {
    router.push(
      appState && appState.targetUrl
      ? appState.targetUrl
      : window.location.pathname
    )
  }
})

extend('required', {
  validate: value => !!value, // the validation function
  message: 'This field is required' // the error message
})
extend('min', min)
extend('max', max)
extend('email', email)
extend('numeric', numeric)
extend('confirmed', confirmed)
extend('regex', regex)
Vue.component('ValidationProvider', ValidationProvider)

Vue.config.productionTip = false

Vue.use(Loading)

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app')
