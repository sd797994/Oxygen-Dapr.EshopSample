import request from '@/utils/request'

export function GetOrderList(data) {
  return request({
    url: '/goodsservice/method/%xxx%query/GetOrderList',
    method: 'post',
    data
  })
}
export function CreateOrder(data) {
  return request({
    url: '/goodsservice/method/%xxx%usecase/CreateOrder',
    method: 'post',
    data
  })
}
export function DeleteOrder(data) {
  return request({
    url: '/goodsservice/method/%xxx%usecase/DeleteOrder',
    method: 'post',
    data
  })
}
export function UpdateOrder(data) {
  return request({
    url: '/goodsservice/method/%xxx%usecase/UpdateOrder',
    method: 'post',
    data
  })
}
