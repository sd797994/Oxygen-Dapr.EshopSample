import request from '@/utils/request'

export function Initpermission() {
  return request({
    url: '/accountservice/permissionquery/GetInitPermissionApilist',
    method: 'post'
  })
}

export function Savepermission(data) {
  return request({
    url: '/accountservice/permissionusecase/SavePermissions',
    method: 'post',
    data
  })
}

export function Getpermission(data) {
  return request({
    url: '/accountservice/permissionquery/GetPermissionList',
    method: 'post',
    data
  })
}

export function GetAllPermissions() {
  return request({
    url: '/accountservice/permissionquery/GetAllPermissions',
    method: 'post'
  })
}

export function GetPermissionRouter() {
  return request({
    url: '/accountservice/permissionquery/GetUserRouter',
    method: 'post'
  })
}
