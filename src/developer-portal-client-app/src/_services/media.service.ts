import { authHeader } from '../_helpers'
import axios from 'axios'

export const mediaService = {
  saveFile
}

async function saveFile(formData: FormData, projectId: string) {
  return await authHeader().then(header => {
    return axios.post(`${process.env.VUE_APP_ROOT_API}/media/save/${projectId}`,
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
