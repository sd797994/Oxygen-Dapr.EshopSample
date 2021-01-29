import request from '@/utils/request'

export function GetLimitedTimeActivitieList(data) {
  return request({
    url: '/goodsservice/activitiquery/GetLimitedTimeActivitieList',
    method: 'post',
    data
  })
}
export function CreateLimitedTimeActivitie(data) {
  return request({
    url: '/goodsservice/activitiusecase/CreateLimitedTimeActivitie',
    method: 'post',
    data
  })
}
export function DeleteLimitedTimeActivitie(data) {
  return request({
    url: '/goodsservice/activitiusecase/DeleteLimitedTimeActivitie',
    method: 'post',
    data
  })
}
export function UpdateLimitedTimeActivitie(data) {
  return request({
    url: '/goodsservice/activitiusecase/UpdateLimitedTimeActivitie',
    method: 'post',
    data
  })
}
export function UpOrDownShelfActivitie(data) {
  return request({
    url: '/goodsservice/activitiusecase/UpOrDownShelfActivitie',
    method: 'post',
    data
  })
}
