import request from '@/utils/request'

export function GetOrderList(data) {
  return request({
    url: '/tradeservice/orderquery/GetOrderList',
    method: 'post',
    data
  })
}
