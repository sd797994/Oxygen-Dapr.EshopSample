import request from '@/utils/request'

export function Getrolelist(data) {
  return request({
    url: '/accountservice/method/rolequery/GetRoleList',
    method: 'post',
    data
  })
}

export function GetAllRoles() {
  return request({
    url: '/accountservice/method/rolequery/GetAllRoles',
    method: 'post'
  })
}

export function RoleCreate(data) {
  return request({
    url: '/accountservice/method/roleusecase/RoleCreate',
    method: 'post',
    data
  })
}

export function RoleUpdate(data) {
  return request({
    url: '/accountservice/method/roleusecase/RoleUpdate',
    method: 'post',
    data
  })
}

export function RoleDelete(data) {
  return request({
    url: '/accountservice/method/roleusecase/RoleDelete',
    method: 'post',
    data
  })
}
