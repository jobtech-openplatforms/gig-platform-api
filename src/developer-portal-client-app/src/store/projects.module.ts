import { projectsService } from '../_services'
import { mediaService } from '../_services'

import { Module, ActionTree, MutationTree, GetterTree } from 'vuex'
import { RootState, UserState } from '@/types'
import { router } from '../_helpers'
import Vue from 'vue'

enum ModuleStatus {
  init,
  ready,
  loading,
  error,
  success,
}

const moduleState: ProjectsModuleState = {
  loading: false,
  all: { projects: [], testProjects: [] },
  current: {},
  editing: {},
  admin: {},
  test: { error: {}, result: {} },
  status: ModuleStatus.init,
  testStatus: ModuleStatus.init,
  testMode: false
}

const actions: ActionTree<ProjectsModuleState, RootState> = {
  getAll({ state, commit, dispatch }, currentProject) {
    if (state.loading) {
      console.log("Project module loading. Not loading projects."+(new Date().getSeconds())+'.'+(new Date().getMilliseconds()))
      return
    }
    commit('getAllRequest')
    return projectsService
            .getAll()
            .then(
              (allProjects) => {
                commit('getAllSuccess', allProjects)
                if (!currentProject) {
                  const p = localStorage.getItem('projectId')
                  if (!state.testMode && localStorage.getItem('testMode') && p.startsWith('Test')) {
                    commit('switchMode')
                  }
                  currentProject = allProjects.projects.find(obj => obj.id === p) ||
                                    allProjects.testProjects.find(obj => obj.id === p)
                  commit('changeCurrentProject', currentProject)
                }
                return currentProject
              },
              (error) => commit('getAllFailure', error)
            )
            .then(
              (currentProject) => {
                dispatch('actionsAfterInit')
                return currentProject
              },
              (error) => error
            )
  },

  testPlatform({ commit }, testData) {
    commit('testStart', ModuleStatus.loading)
    projectsService
      .testApi(testData)
      .then(
        (testResult) => commit('testSuccess', testResult),
        (error) => commit('testFailure', error)
      )
  },

  goLive({ commit, dispatch }, projectId: string) {
    commit('goLiveStart', ModuleStatus.loading)
    projectsService
      .goLive(projectId)
      .then(
        (result) => {
          commit('goLiveSuccess', result)
          dispatch('getAll')
          return result || true
        },
        (error) => commit('goLiveFailure', error)
      )
  },

  createProject({ state, commit, dispatch }, projectData) {
    commit('getRequest', projectData)

    return projectsService
      .create({...projectData, testMode: state.testMode })
      .then(
        (currentProject) => {
          commit('getSuccess', currentProject)
          commit('changeCurrentProject', currentProject)
          dispatch('getAll', currentProject)
          return currentProject
        },
        (error) => {
            commit('getFailure', error)
            throw error
          }
    )
  },

  updateProject({ state, commit, dispatch }, projectData) {
    commit('getRequest', projectData)
    projectData.id = state.current.project.id

    return projectsService
      .update(projectData)
      .then(
        (currentProject) => {
          commit('getSuccess', currentProject)
          dispatch('getAll')
        },
        (error) =>
          commit('getFailure', error)
      )
  },

  getProject({ state, commit, dispatch }, id) {
    commit('getRequest', id)

    projectsService
      .getProject(id)
      .then(
        (currentProject) => {
          dispatch('actionsAfterInit')
          commit('getSuccess', currentProject)
        },
        (error) => commit('getFailure', error)
      )
  },

  setPlatformUrl({ state, commit, dispatch }, url: string) {
    commit('getRequest', state.current.project.id)
    if(!url){
      commit('getFailure', {errors: ['platform-url'], message: 'Please add a new url.'})
    }
    return projectsService
            .setPlatformUrl(state.current.project.id, url, state.testMode)
            .then(
              (currentProject) => commit('getSuccess', currentProject),
              (error) => commit('getFailure', error)
            )
  },

  createApplication({ state, commit }) {
    console.log("createApplication"+(new Date().getSeconds())+'.'+(new Date().getMilliseconds()))
    if (state.current && state.current.project && state.current.project.applications && state.current.project.applications.length>0) {
      return state.current.project
    }
    commit('getRequest', state.current.project.id)
    projectsService
      .createApplication(state.current.project.id)
      .then(
        (currentProject) => {
          commit('getSuccess', currentProject)
          return currentProject
        },
        (error) => commit('getFailure', error)
      )
  },

  setApplicationUrls({ state, commit }, urls: ApplicationUrlsUpdateRequest) {
    commit('getRequest', state.current.project.id)
    projectsService
      .setApplicationUrls(state.current.project.id, urls)
      .then(
        (currentProject) => {
          commit('getSuccess', currentProject)
          // router.push('/project').catch(err => { })
          return currentProject
        },
        (error) => commit('getFailure', error)
      )
  },

  resetPlatformTest({ commit }) {
    commit('resetTest')
  },

  initCurrentProject({ state, commit, dispatch }, thenDispatch: string) {
    console.log("initCurrentProject "+(new Date().getSeconds())+'.'+(new Date().getMilliseconds()))
    if (state.current && state.current.project) {
      return state.current.project
    }
    const p = localStorage.getItem('projectId')
    if (!p || p === 'undefined') {
      dispatch('unsetCurrentProject')
      // tslint:disable-next-line:no-empty
      router.push('/projects').catch(err => { })
    }
    
    dispatch('actionsAfterInit')

    return state.current ? state.current.project : null
  },

  setCurrentProject({ commit }, project) {
    if (!project || project === 'undefined') {
      return // throw error?
    }
    commit('changeCurrentProject', project)
    localStorage.setItem('projectId', project.id)
    commit('resetTest', ModuleStatus.ready)
    // tslint:disable-next-line:no-empty
    router.push('/project').catch(err => { })
  },

  unsetCurrentProject({ commit }) {
    commit('changeCurrentProject', null)
    localStorage.removeItem('projectId')
    return
  },
  actionsAfterInit({state, dispatch, commit}) {
    console.log("actionsAfterInit "+(new Date().getSeconds())+'.'+(new Date().getMilliseconds()))
    if (state.dispatchAfterInit) {
      state.dispatchAfterInit.forEach(action => {
        dispatch(action)
      });
      commit('clearDispatchAfterInit')
    }
  },
}

const mutations: MutationTree<ProjectsModuleState> = {
  getAllRequest(state) {
    state.loading = true
  },
  getAllSuccess(state, allProjects) {
    Vue.set(state, 'loading', false)
    Vue.set(state, 'all', allProjects)
  },
  getAllFailure(state, error) {
    Vue.set(state, 'loading', false)
    state.all.error = error
  },
  getRequest(state, project) {
    Vue.set(state, 'loading', true)
    state.status = ModuleStatus.loading
    Vue.set(state.current, 'error', null)
  },
  getSuccess(state, currentProject) {
    Vue.set(state.current, 'project', currentProject)
    Vue.set(state.editing, 'project', currentProject)
    Vue.set(state, 'loading', false)
    state.status = ModuleStatus.ready
    state.testStatus = ModuleStatus.ready
    localStorage.setItem('projectId', currentProject.id)
    return currentProject
  },
  getFailure(state, error) {
    Vue.set(state, 'loading', false)
    state.status = ModuleStatus.error
    state.current.error = error
  },
  resetTest(state, status) {
    Vue.set(state, 'loading', false)
    state.test.result = {}
    state.test.error = {}
    state.testStatus = status || ModuleStatus.ready
  },
  testStart(state, status) {
    Vue.set(state, 'loading', true)
    state.test.result = {}
    state.test.error = {}
    state.testStatus = status || ModuleStatus.loading
    // return error
  },
  testSuccess(state, testData) {
    Vue.set(state, 'loading', false)
    state.test.result = testData
    state.test.error = {}
    state.testStatus = ModuleStatus.success
    return testData
  },
  testFailure(state, error) {
    Vue.set(state, 'loading', false)
    state.test.error = error
    state.testStatus = ModuleStatus.error
    // return error
  },
  goLiveStart(state, status) {
    Vue.set(state, 'loading', true)
    state.status = status || ModuleStatus.loading
    // return error
  },
  goLiveSuccess(state, result) {
    Vue.set(state.current, 'project', result)
    Vue.set(state, 'loading', false)
    state.status = ModuleStatus.success
    return result
  },
  goLiveFailure(state, error) {
    Vue.set(state, 'loading', false)
    state.status = ModuleStatus.error
    Vue.set(state.current, 'error', error)

  },
  changeCurrentProject(state, project) {
    Vue.set(state.current, 'error', null)
    if (project) {
      localStorage.setItem('projectId', project.id)
    }
    Vue.set(state.current, 'project', project)
    Vue.set(state.editing, 'project', project)
  },
  localEdit(state, edited) {
    state.editing.project[edited.name] = edited.value
    Vue.set(state.editing, 'project', state.editing.project)
  },
  cancelEdit(state) {
    Vue.set(state.editing, 'project', state.current.project)
  },
  switchMode(state) {
    if (state.current.project) {
      // Get same project from other mode
      var project =
      (state.testMode) ?
      // Get live project id
      state.all.projects.find(obj => obj.id == state.current.project.liveProjectId)
      :
      // Get test project by live id
      state.all.testProjects.find(obj => obj.liveProjectId === state.current.project.id)
      
      if (project) {
        // TODO: Same as changeCurrentProject above, duplicated code. How to fix in Vuex modules w/ TS?
        Vue.set(state.test, 'result', null)
        Vue.set(state.test, 'status', ModuleStatus.ready)
        Vue.set(state.current, 'error', null)
        localStorage.setItem('projectId', project.id)
        Vue.set(state.current, 'project', project)
        Vue.set(state.editing, 'project', project)
        router.push('/project').catch(err => { })
      }
    }
    Vue.set(state, 'testMode', !state.testMode)
    localStorage.setItem('testMode', state.testMode ? '1':'')
  },
  queueDispatchAfterInit(state, action) {
    console.log('queueDispatchAfterInit '+(new Date().getSeconds())+'.'+(new Date().getMilliseconds()))
    state.dispatchAfterInit = state.dispatchAfterInit || []
    if(!state.dispatchAfterInit.includes(action))
      state.dispatchAfterInit.push(action)
  },
  clearDispatchAfterInit(state) {
    Vue.set(state, 'dispatchAfterInit', [])
  }

}

const getters: GetterTree<ProjectsModuleState, RootState> = {
  hasProjects(state) {
    return state.all && state.all.projects && state.all.projects.length > 0
  },
  currentProject(state) {
    return state.current.project
  },
  currentProjects(state) {
    return state.testMode ? state.all.testProjects : state.all.projects
  },
  currentProjectCompleted(state) {
    return !state.testMode &&(state.current.project &&
      state.current.project.name &&
      state.current.project.webpage &&
      state.current.project.logoUrl &&
      state.current.project.description)
  },
  nextStep(state) {
    if(state.current.project && !state.current.project.platforms[0].exportDataUri)
      return 'platformDataUrlIncomplete'
    if (state.testMode){
      if(state.current.project &&
      state.current.project.name &&
      state.current.project.webpage &&
      state.current.project.logoUrl &&
      state.current.project.description) {
      return 'testModeComplete'
    }else{
      return 'testModeIncomplete'
    }
  }
    if (!(state.current.project &&
      state.current.project.name &&
      state.current.project.webpage &&
      state.current.project.logoUrl &&
      state.current.project.description)) {
      return 'currentProjectIncomplete'
    }
    return ''
  },
  currentError(state) {
    return state.current.error
  },
  editingProject(state) {
    return state.editing.project
  },
  testResult(state) {
    return state.test.result
  },
  testError(state) {
    return state.test.error && state.test.error.message ? state.test.error.message : state.test.error
  },
  testStatus(state) {
    return state.status.toString()
  },
  currentPlatform(state) {
    return state.current.project && state.current.project.platforms  && state.current.project.platforms.length > 0 ? state.current.project.platforms[0] : null
  },
  currentApplication(state) {
    return state.current.project && state.current.project.applications && state.current.project.applications.length > 0 ? state.current.project.applications[0] : null
  },
  loading(state) {
    return state.loading
  }
}

export const projects: Module<ProjectsModuleState, RootState> = {
  namespaced: true,
  state: moduleState,
  actions,
  mutations,
  getters
}

// Guessing... :-D
// export interface TestProjectState extends ProjectState {
//   liveProjectId: string
// }

export interface ProjectState extends ProjectUpdateRequest {
  liveProjectId?: string // Only available on TestProject
  platforms?: PlatformState[]
  applications?: ApplicationState[]
  platformToken?: string
}

export interface ApplicationState {
  emailVerificationUrl?: string
  gigDataNotificationUrl?: string
  authCallbackUrl?: string
  id: string
  secretKey: string
}
export interface PlatformState extends PlatformUpdateRequest {
  platformToken?: string
}

export interface PlatformUpdateRequest {
  id?: string
  exportDataUri?: string
}

export interface ProjectUpdateRequest {
  id?: string
  name: string
  webpage?: string
  description?: string
  logoUrl?: string
}

export interface ApplicationUrlsUpdateRequest {
  authCallbackUrl: string
  gigDataNotificationUrl: string
  emailVerificationUrl: string
}

export interface AllPlatformsState extends BasicState {
  projects?: ProjectState[],
  testProjects?: ProjectState[]
}

export interface CurrentPlatformState extends BasicState {
  project?: ProjectState,
  // testProject?: ProjectState
}

export interface AdminUserState extends BasicState {
  user?: UserState
}

export interface CreateRequest {
  name: string
  testMode: boolean
}

export interface TestRequest {
  id: string
  username: string
}

export interface TestState extends BasicState {
  // tslint:disable-next-line:no-any
  error?: any
  // tslint:disable-next-line:no-any
  result?: any
}

interface ErrorState {
  errors?: boolean
  message?: string
}

interface BasicState {
  error?: string | ErrorState
}

export interface ProjectsModuleState {
  dispatchAfterInit?: string[]
  loading: boolean
  current: CurrentPlatformState
  editing: CurrentPlatformState
  all: AllPlatformsState
  admin: AdminUserState
  test: TestState
  status: ModuleStatus
  testStatus: ModuleStatus
  testMode: boolean
}
