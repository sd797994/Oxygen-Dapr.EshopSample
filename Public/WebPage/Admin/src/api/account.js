import request from '@/utils/request'

export function fetchList(data) {
  return request({
    url: '/accountservice/accountquery/GetAccountList',
    method: 'post',
    data
  })
}

export function lockaccount(data) {
  return request({
    url: '/accountservice/accountusecase/LockOrUnLockAccount',
    method: 'post',
    data
  })
}

export function accountCreate(data) {
  return request({
    url: '/accountservice/accountusecase/AccountCreate',
    method: 'post',
    data
  })
}

export function accountDelete(data) {
  return request({
    url: '/accountservice/accountusecase/AccountDelete',
    method: 'post',
    data
  })
}

export function accountUpdate(data) {
  return request({
    url: '/accountservice/accountusecase/accountUpdate',
    method: 'post',
    data
  })
}
