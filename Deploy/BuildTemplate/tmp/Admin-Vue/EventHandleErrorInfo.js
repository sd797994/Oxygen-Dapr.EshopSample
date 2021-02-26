import request from '@/utils/request'

export function GetEventHandleErrorInfoList(data) {
  return request({
    url: '/goodsservice/method/%xxx%query/GetEventHandleErrorInfoList',
    method: 'post',
    data
  })
}
export function CreateEventHandleErrorInfo(data) {
  return request({
    url: '/goodsservice/method/%xxx%usecase/CreateEventHandleErrorInfo',
    method: 'post',
    data
  })
}
export function DeleteEventHandleErrorInfo(data) {
  return request({
    url: '/goodsservice/method/%xxx%usecase/DeleteEventHandleErrorInfo',
    method: 'post',
    data
  })
}
export function UpdateEventHandleErrorInfo(data) {
  return request({
    url: '/goodsservice/method/%xxx%usecase/UpdateEventHandleErrorInfo',
    method: 'post',
    data
  })
}
