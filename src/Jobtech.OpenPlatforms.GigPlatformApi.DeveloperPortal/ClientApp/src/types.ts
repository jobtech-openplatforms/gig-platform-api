// Store

export interface RootState {
  session: SessionState
}
// Models
export interface SessionState {
  session: Session
}

export interface Session {
  email: string
  sessionKey: string
  isLoggedIn: boolean
}

export interface LoginState {
  user: string
  isLoggedIn: boolean
}

export interface UserState {
  email: string
  id: string
  name: string
  token?: string
}
