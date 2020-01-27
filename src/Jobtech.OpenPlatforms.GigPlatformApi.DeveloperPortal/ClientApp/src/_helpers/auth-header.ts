import { getInstance } from '../auth/auth-wrapper'

export async function authHeader() {

  const authService = getInstance()
  const token = await authService.getTokenSilently()

  if (token) {
    return { Authorization: 'Bearer ' + token }
  } else {
    return {}
  }
}

export function removeIdNamespace(id: string) {
  return id.split(/\//).pop()
}
