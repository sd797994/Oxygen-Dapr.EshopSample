import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/accountservice/accountusecase/AccountLogin',
    method: 'post',
    data
  })
}

export function getInfo() {
  return request({
    url: '/accountservice/accountquery/GetAccountInfo',
    method: 'post'
  })
}

export function logout() {
  return request({
    url: '/accountservice/accountusecase/AccountLoginOut',
    method: 'post'
  })
}

export function checkRolebasedInit() {
  return request({
    url: '/accountservice/accountquery/CheckRoleBasedAccessControler',
    method: 'post'
  })
}

export function InitRoleBasedAccessControler(data) {
  return request({
    url: '/accountservice/accountusecase/InitRoleBasedAccessControler',
    method: 'post',
    data
  })
}

export function updateuser(data) {
  return request({
    url: '/accountservice/accountusecase/SupplementaryAccountInfo',
    method: 'post',
    data
  })
}
