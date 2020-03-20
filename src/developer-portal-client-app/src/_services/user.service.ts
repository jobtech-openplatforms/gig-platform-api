import { authHeader } from '../_helpers'

export const userService = {
    login,
    logout,
    register,
    resetLogin,
    getAll,
    getById,
    update,
    delete: _delete
}

function login(email, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    }

    return fetch(`${process.env.VUE_APP_ROOT_API}/platformadmins/authenticate`, requestOptions)
        .then(handleResponse)
        .then((user) => {
            // login successful if there's a jwt token in the response
            if (user.token) {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('user', JSON.stringify(user))
            }

            return user
        })
}

function logout() {
    // remove user from local storage to log user out
  localStorage.removeItem('user')
  localStorage.removeItem('project')
}

function register(user) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    }

    return fetch(`${process.env.VUE_APP_ROOT_API}/platformadmins/register`, requestOptions).then(handleResponse)
}

function resetLogin(email) {
  const requestOptions = {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(email)
  }

  return fetch(`${process.env.VUE_APP_ROOT_API}/platformadmins/reset`, requestOptions).then(handleResponse)
}

function getAll() {

    return authHeader().then(header => {
        const requestOptions = {
            method: 'GET',
            headers: header
        }
        return fetch(`${process.env.VUE_APP_ROOT_API}/users`, requestOptions).then(handleResponse)
    })
}

function getById(id) {
    return authHeader().then(header => {
        const requestOptions = {
            method: 'GET',
            headers: header
        }
        return fetch(`${process.env.VUE_APP_ROOT_API}/platformadmins/${id}`, requestOptions).then(handleResponse)
    })

}

function update(user) {

    return authHeader().then(header => {
        const requestOptions = {
            method: 'PUT',
            headers: { ...header, 'Content-Type': 'application/json' },
            body: JSON.stringify(user)
        }

        return fetch(`${process.env.VUE_APP_ROOT_API}/platformadmins/${user.id}`, requestOptions)
                .then(handleResponse)
                .then((userResponse) => {
                  const usr = JSON.parse(localStorage.getItem('user'))
                  localStorage.removeItem('user')
                  usr.email = userResponse.email
                  usr.name = userResponse.name
                  localStorage.setItem('user', JSON.stringify(usr))
                })
    })
}

// prefixed function name with underscore because delete is a reserved word in javascript
function _delete(id) {

    return authHeader().then(header => {
        const requestOptions = {
            method: 'DELETE',
            headers: header
        }
        return fetch(`${process.env.VUE_APP_ROOT_API}/platformadmins/${id}`, requestOptions).then(handleResponse)
    })
}

function handleResponse(response) {
    return response.text().then((text) => {
        const data = text && JSON.parse(text)
        if (!response.ok) {
            if (response.status === 401) {
                // auto logout if 401 response returned from api
                logout()
                location.reload(true)
            }

            const error = (data && data.message) || response.statusText
            return Promise.reject(error)
        }

        return data
    })
}
