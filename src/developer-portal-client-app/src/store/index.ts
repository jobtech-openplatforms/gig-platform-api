import Vue from 'vue'
import Vuex from 'vuex'

import { alert } from './alert.module'
import { account } from './account.module'
import { projects } from './projects.module'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    alert,
    account,
    projects,
  },
})
