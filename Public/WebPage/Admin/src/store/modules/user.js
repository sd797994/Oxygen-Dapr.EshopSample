import { login, logout, getInfo, updateuser, checkRolebasedInit, InitRoleBasedAccessControler } from '@/api/user'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { resetRouter } from '@/router'
import { MessageBox, Message } from 'element-ui'
import Cookies from 'js-cookie'

const getDefaultState = () => {
  return {
    token: getToken(),
    name: '',
    userImage: 'https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif',
    userName: '',
    gender: '',
    state: '',
    birthDay: '',
    address: '',
    tel: '',
    permissions: ''
  }
}

const state = getDefaultState()

const mutations = {
  RESET_STATE: (state) => {
    Object.assign(state, getDefaultState())
  },
  SET_TOKEN: (state, token) => {
    state.token = token
  },
  SET_NAME: (state, name) => {
    state.name = name
  },
  SET_AVATAR: (state, avatar) => {
    state.avatar = avatar
  },
  SET_USER: (state, user) => {
    state.name = user.nickName
    state.userName = user.userName
    state.userImage = user.userImage ?? state.userImage
    state.gender = user.gender
    state.state = user.state
    state.birthDay = user.birthDay
    state.address = user.address
    state.tel = user.tel
    state.permissions = user.permissions
  }
}

const actions = {
  checkRolebasedInit() {
    return new Promise((resolve, reject) => {
      checkRolebasedInit().then(response => {
        if (response.data === false) {
          MessageBox.confirm('系统尚未进行权限初始化，是否开始初始化?', '权限初始化检查', {
            confirmButtonText: '初始化',
            cancelButtonText: '取消',
            type: 'warning'
          }).then(() => {
            var cookie = Cookies.get('githubuser')
            InitRoleBasedAccessControler({ OauthData: cookie }).then((response) => {
              Message({
                message: response.message,
                type: 'success',
                duration: 5 * 1000
              })
              resolve(response)
            }, msg => { })
          }, msg => { })
        } else {
          resolve(response)
        }
      }, msg => { }).catch(error => {
        reject(error)
      })
    })
  },

  // user login
  login({ commit }, userInfo) {
    const { username, password } = userInfo
    return new Promise((resolve, reject) => {
      login({ loginName: username.trim(), password: password, loginAdmin: true }).then(response => {
        const { data } = response
        commit('SET_TOKEN', data.token)
        setToken(data.token)
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // get user info
  getInfo({ commit, state }) {
    return new Promise((resolve, reject) => {
      getInfo().then(response => {
        const { data } = response
        if (!data) {
          return reject('Verification failed, please Login again.')
        }
        commit('SET_USER', data)
        resolve(data)
      }).catch(error => {
        reject(error)
      })
    })
  },

  // user logout
  logout({ commit, state }) {
    return new Promise((resolve, reject) => {
      logout(state.token).then(() => {
        removeToken() // must remove  token  first
        resetRouter()
        commit('RESET_STATE')
        resolve()
      }, msg => {
        removeToken() // must remove  token  first
        resetRouter()
        commit('RESET_STATE')
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // remove token
  resetToken({ commit }) {
    return new Promise(resolve => {
      removeToken() // must remove  token  first
      commit('RESET_STATE')
      resolve()
    })
  },

  updateuser({ commit, state }) {
    return new Promise((resolve, reject) => {
      updateuser(state).then((response) => {
        Message({
          message: response.message,
          type: 'success',
          duration: 5 * 1000
        })
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
