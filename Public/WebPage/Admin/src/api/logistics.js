import request from '@/utils/request'

export function GetLogisticsList(data) {
  return request({
    url: '/tradeservice/logisticsquery/GetLogisticsList',
    method: 'post',
    data
  })
}
export function CreateDeliver(data) {
  return request({
    url: '/tradeservice/logisticsusecase/Deliver',
    method: 'post',
    data
  })
}
export function CreateReceive(data) {
  return request({
    url: '/tradeservice/logisticsusecase/Receive',
    method: 'post',
    data
  })
}
