import { authHeader } from '../_helpers'
import axios from 'axios'

const baseUrl = `${process.env.VUE_APP_ROOT_API}/test/app/`

export const applicationTestService = {
  testAuthentication,
  testAuthenticationCancel,
  testData
}

async function testAuthentication(projectId: string) {
  return await get(`${baseUrl}${projectId}/auth`)
}

async function testAuthenticationCancel (projectId: string) {
  return await get(`${baseUrl}${projectId}/auth/cancel`)
}

async function testData (projectId: string) {
  return await get(`${baseUrl}${projectId}/data`)
}

async function post (formData: FormData, url: string) {
  return await authHeader().then(header => {
    return axios.post(`${url}`,
      formData,
      {
        headers: {
          ...header,
          'Content-Type': 'multipart/form-data'
        }
      }
    )
  })
}

async function get (url: string) {
  return await authHeader().then(header => {
    return axios.get(`${url}`,
      {
        headers: {
          ...header,
          'Content-Type': 'multipart/form-data'
        }
      }
    )
  })
}
