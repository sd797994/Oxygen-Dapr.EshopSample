import request from '@/utils/request'

export function GetLimitedTimeActivitieList(data) {
  return request({
    url: '/goodsservice/method/activitiquery/GetLimitedTimeActivitieList',
    method: 'post',
    data
  })
}
export function CreateLimitedTimeActivitie(data) {
  return request({
    url: '/goodsservice/method/activitiusecase/CreateLimitedTimeActivitie',
    method: 'post',
    data
  })
}
export function DeleteLimitedTimeActivitie(data) {
  return request({
    url: '/goodsservice/method/activitiusecase/DeleteLimitedTimeActivitie',
    method: 'post',
    data
  })
}
export function UpdateLimitedTimeActivitie(data) {
  return request({
    url: '/goodsservice/method/activitiusecase/UpdateLimitedTimeActivitie',
    method: 'post',
    data
  })
}
export function UpOrDownShelfActivitie(data) {
  return request({
    url: '/goodsservice/method/activitiusecase/UpOrDownShelfActivitie',
    method: 'post',
    data
  })
}
