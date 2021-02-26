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
