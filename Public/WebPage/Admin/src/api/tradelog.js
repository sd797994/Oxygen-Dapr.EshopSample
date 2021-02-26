import request from '@/utils/request'

export function GetTradeLogListByOrderId(data) {
  return request({
    url: '/tradeservice/tradelogquery/GetTradeLogListByOrderId',
    method: 'post',
    data
  })
}
