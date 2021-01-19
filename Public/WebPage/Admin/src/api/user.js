import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/accountservice/method/accountusecase/AccountLogin',
    method: 'post',
    data
  })
}

export function getInfo() {
  return request({
    url: '/accountservice/method/accountquery/GetAccountInfo',
    method: 'post'
  })
}

export function logout() {
  return request({
    url: '/accountservice/method/accountusecase/AccountLoginOut',
    method: 'post'
  })
}

export function checkRolebasedInit() {
  return request({
    url: '/accountservice/method/accountquery/CheckRoleBasedAccessControler',
    method: 'post'
  })
}

export function InitRoleBasedAccessControler() {
  return request({
    url: '/accountservice/method/accountusecase/InitRoleBasedAccessControler',
    method: 'post'
  })
}

export function updateuser(data) {
  return request({
    url: '/accountservice/method/accountusecase/SupplementaryAccountInfo',
    method: 'post',
    data
  })
}
