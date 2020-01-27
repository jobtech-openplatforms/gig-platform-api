import { userService, projectsService } from '../_services'
import { router } from '../_helpers'

const user = JSON.parse(localStorage.getItem('user'))
const moduleState = user
  ? { status: { loggedIn: true }, user }
  : { status: {}, user: null }

const actions = {
  login({ dispatch, commit }, { email, password }) {
    commit('loginRequest', { email })

    userService.login(email, password).then(
      (userObj) => {
        commit('loginSuccess', userObj)
        // tslint:disable-next-line:no-empty
        router.push('/projects').catch(err => {})
      },
      (error) => {
        commit('loginFailure', error)
        dispatch('alert/error', error, { root: true })
      }
    )
  },
  logout({ commit }) {
    userService.logout()
    commit('logout')
  },
  register({ dispatch, commit }, userObj) {
    commit('registerRequest', userObj)

    userService.register(userObj).then(
      (theUser) => {
        commit('registerSuccess', theUser)
        router.push('/login')
        setTimeout(() => {
          // display success message after route change completes
          dispatch('alert/success', 'Registration successful', { root: true })
        })
      },
      (error) => {
        commit('registerFailure', error)
        dispatch('alert/error', error, { root: true })
      }
    )
  },
  resetLogin({ dispatch, commit }, email) {
    // commit('registerRequest', userObj)

    userService.resetLogin(email).then(
      (response) => {
        // commit('registerSuccess', theUser)
        // router.push('/login')
        setTimeout(() => {
          // display success message after route change completes
          dispatch('alert/success',
          'Let us pretend that you received an e-mail with this link to reset your password: <a href="/reset-password?code=' +
          response.resetCode +
          '">Click to reset</a>', { root: true })
        })
      },
      (error) => {
        commit('registerFailure', error)
        dispatch('alert/error', error, { root: true })
      }
    )
  },
  updateContact({ dispatch, commit }, contact) {
    projectsService.updateContact(contact).then(
      (contactUser) => {
        commit('updateSuccess', contactUser)
      },
      (error) => {
        // dispatch('alert/error', error, { root: true })
        if (error === 'Unauthorized') {
          router.push('/login')
        }
      }
    )
  }
}

const mutations = {
  loginRequest(state, userObj) {
    state.status = { loggingIn: true }
    state.user = userObj
  },
  loginSuccess(state, userObj) {
    state.status = { loggedIn: true }
    state.user = userObj
  },
  loginFailure(state) {
    state.status = {}
    state.user = null
  },
  logout(state) {
    state.status = {}
    state.user = null
  },
  registerRequest(state, userObj) {
    state.status = { registering: true }
  },
  registerSuccess(state, userObj) {
    state.status = {}
  },
  registerFailure(state, error) {
    state.status = {}
  },
  updateSuccess(state, userObj) {
    state.user = userObj
    const usr = JSON.parse(localStorage.getItem('user'))
    // localStorage.removeItem('user')
    usr.email = userObj.email
    usr.name = userObj.name
    localStorage.setItem('user', JSON.stringify(usr))
  }
}

const getters = {
  user() {
    return user
  }
}

export const account = {
  namespaced: true,
  state: moduleState,
  getters,
  actions,
  mutations
}
