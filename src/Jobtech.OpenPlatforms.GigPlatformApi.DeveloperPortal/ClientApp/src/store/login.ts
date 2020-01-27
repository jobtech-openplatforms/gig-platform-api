import { GetterTree, MutationTree, ActionTree } from 'vuex'
import { LoginState, RootState, Session } from '../types'

const moduleState: LoginState = {
  user: '',
  isLoggedIn: false
}

const mutations: MutationTree<LoginState> = {
  login(state) {
    state.isLoggedIn = true
    state.user = 'Alex'
  }
}

const getters: GetterTree<LoginState, RootState> = {}

const actions: ActionTree<LoginState, RootState> = {
  login({ commit }, newSession: Session) {
    commit('login', newSession)
  }
}

export const login = {
  state: moduleState,
  getters,
  mutations,
  actions,
  namespaced: true
}
