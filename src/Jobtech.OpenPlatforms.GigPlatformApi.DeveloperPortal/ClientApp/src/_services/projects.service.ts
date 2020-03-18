import { authHeader, removeIdNamespace } from '../_helpers'
import { ProjectState,
  ProjectUpdateRequest,
  ApplicationUrlsUpdateRequest,
  TestRequest,
  CreateRequest } from '@/store/projects.module'
import { UserState } from '@/types'

export const projectsService = {
  create,
  getProject,
  testApi,
  goLive,
  getAll,
  update,
  updateContact,
  setPlatformUrl,
  setApplicationUrls
}

async function create(projectData: CreateRequest) {

  const header = await authHeader()

  const requestOptions = {
    method: 'POST',
    headers: { ...header, 'Content-Type': 'application/json' },
    body: JSON.stringify(projectData)
  }

  const response = await fetch(
    `${
    process.env.VUE_APP_ROOT_API
    }/projects/create`,
    requestOptions
  )
  return handleResponse(response)
}

async function getAll() {

  const header = await authHeader()

  const requestOptions = {
    method: 'GET',
    headers: { ...header, 'Content-Type': 'application/json' }
  }

  const response = await fetch(
    `${process.env.VUE_APP_ROOT_API}/projects`,
    requestOptions
  )
  return handleResponse(response)
}

async function getProject(id: string) {
  const header = await authHeader()

  const requestOptions = {
    method: 'GET',
    headers: { ...header, 'Content-Type': 'application/json' }
  }

  const response = await fetch(
    `${
      process.env.VUE_APP_ROOT_API
    }/platformadmins/platform/${removeIdNamespace(id)}`,
    requestOptions
  )
  return handleResponse(response)
}

async function setPlatformUrl(projectId: string, url: string, testMode: boolean) {
  const header = await authHeader()

  const requestOptions = {
    method: 'POST',
    headers: { ...header, 'Content-Type': 'application/json' },
    body: JSON.stringify({projectId, url, testMode})
  }

  const response = await fetch(
    `${
      process.env.VUE_APP_ROOT_API
    }/projects/platformurl`,
    requestOptions
  )
  return handleResponse(response)
}

async function setApplicationUrls(projectId: string, urls: ApplicationUrlsUpdateRequest) {
  const header = await authHeader()

  const requestOptions = {
    method: 'POST',
    headers: { ...header, 'Content-Type': 'application/json' },
    body: JSON.stringify({ projectId, ...urls })
  }

  const response = await fetch(
    `${
    process.env.VUE_APP_ROOT_API
    }/projects/applicationUrls`,
    requestOptions
  )
  return handleResponse(response)
}

async function testApi(testData: TestRequest) {
  const header = await authHeader()

  const requestOptions = {
    method: 'POST',
    headers: { ...header, 'Content-Type': 'application/json' },
    body: JSON.stringify(testData)
  }

  const response = await fetch(
    `${
    process.env.VUE_APP_ROOT_API
    }/platform/test/${removeIdNamespace(testData.id)}`,
    requestOptions
  )
  return handleResponse(response)
}

async function goLive(projectId: string) {
  const header = await authHeader()
  const requestOptions = {
    method: 'POST',
    headers: { ...header, 'Content-Type': 'application/json' },
    body: JSON.stringify({ projectId, status: 'activate'})
  }

  const response = await fetch(
    `${
    process.env.VUE_APP_ROOT_API
    }/projects/platformlive`,
    requestOptions
  )
  return handleResponse(response)
}

async function update(project: ProjectUpdateRequest) {
  const request = {
    id: project.id,
    name: project.name,
    webpage: project.webpage,
    description: project.description,
    logoUrl: project.logoUrl
  }

  const header = await authHeader()

  const requestOptions: RequestInit = {
    method: 'POST',
    headers: { ...header, 'Content-Type': 'application/json' },
    body: JSON.stringify(request)
  }

  const response = await fetch(
    `${
      process.env.VUE_APP_ROOT_API
    }/projects/update`,
    requestOptions
  )
  return handleResponse(response)
}

async function updateContact(user: UserState) {

  const header = await authHeader()

  const requestOptions = {
    method: 'PUT',
    headers: { ...header, 'Content-Type': 'application/json' },
    body: JSON.stringify(user)
  }

  const response = await fetch(
    `${process.env.VUE_APP_ROOT_API}/platformadmins/contact/${removeIdNamespace(
      user.id
    )}`,
    requestOptions
  )
  return handleResponse(response)
}

// TODO: Make typed response data
function handleResponse(response: Response) {
  return response.text().then((text) => {
    const data = text && JSON.parse(text)
    if (!response.ok) {
      // const error = (data && data.message) || response.statusText
      // return Promise.reject(error)
      return Promise.reject(data)
    }

    return data
  })
}
