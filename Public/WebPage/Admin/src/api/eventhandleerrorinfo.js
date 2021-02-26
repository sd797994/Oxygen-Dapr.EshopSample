import request from '@/utils/request'

export function GetEventHandleErrorInfoList(data) {
  return request({
    url: '/publicservice/eventhandleerrorinfoquery/GetEventHandleErrorInfoList',
    method: 'post',
    data
  })
}
