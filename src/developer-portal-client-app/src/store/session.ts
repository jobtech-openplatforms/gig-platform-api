import { RootState, SessionState, Session } from '@/types'
import { Store, GetterTree, MutationTree, ActionTree, Module } from 'vuex'

const getters: GetterTree<SessionState, RootState> = {

}

const mutations: MutationTree<SessionState> = {
  login(state, userSession: Session) {
    state.session = userSession
  },
}

const actions: ActionTree<SessionState, RootState> = {
  login({commit}, newSession: Session) {
    commit('login', newSession)
  },
}

const moduleState: SessionState = {
  session: {
    email: '',
    sessionKey: '',
    isLoggedIn: false,
  },
}

export const session: Module<SessionState, RootState> = {
  state: moduleState,
  getters,
  mutations,
  actions,
  namespaced:  true,
}

export default session

export function load(store: Store<RootState>) {
  store.registerModule('session', session)
}
