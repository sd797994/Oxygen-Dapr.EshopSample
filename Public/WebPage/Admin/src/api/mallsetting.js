import request from '@/utils/request'

export function GetMallSetting() {
  return request({
    url: '/publicservice/mallsettingquery/GetMallSetting',
    method: 'post'
  })
}
export function CreateOrUpdateMallSetting(data) {
  return request({
    url: '/publicservice/mallsettingusecase/CreateOrUpdateMallSetting',
    method: 'post',
    data
  })
}
export function GetSentinelConfig() {
  return request({
    url: '/publicservice/sentinelconfig/Get',
    method: 'post'
  })
}
export function GetCommonData() {
  return request({
    url: '/publicservice/sentinelconfig/GetCommonData',
    method: 'post'
  })
}
export function SaveSentinelConfig(data) {
  return request({
    url: '/publicservice/sentinelconfig/Save',
    method: 'post',
    data
  })
}
