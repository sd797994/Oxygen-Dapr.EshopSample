import request from '@/utils/request'

export function GetOrderList(data) {
  return request({
    url: '/tradeservice/orderquery/GetOrderList',
    method: 'post',
    data
  })
}

export function OrderPay(data) {
  return request({
    url: '/tradeservice/orderusecase/OrderPay',
    method: 'post',
    data
  })
}
