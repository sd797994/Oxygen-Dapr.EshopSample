import request from '@/utils/request'

export function GetLimitedTimeActivitieList(data) {
  return request({
    url: '/goodsservice/method/LimitedTimeActivitiequery/GetLimitedTimeActivitieList',
    method: 'post',
    data
  })
}
export function CreateLimitedTimeActivitie(data) {
  return request({
    url: '/goodsservice/method/LimitedTimeActivitieusecase/CreateLimitedTimeActivitie',
    method: 'post',
    data
  })
}
export function DeleteLimitedTimeActivitie(data) {
  return request({
    url: '/goodsservice/method/LimitedTimeActivitieusecase/DeleteLimitedTimeActivitie',
    method: 'post',
    data
  })
}
export function UpdateLimitedTimeActivitie(data) {
  return request({
    url: '/goodsservice/method/LimitedTimeActivitieusecase/UpdateLimitedTimeActivitie',
    method: 'post',
    data
  })
}
