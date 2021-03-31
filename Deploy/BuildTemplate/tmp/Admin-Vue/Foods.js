import request from '@/utils/request'

export function GetFoodsList(data) {
  return request({
    url: '/goodsservice/method/%xxx%query/GetFoodsList',
    method: 'post',
    data
  })
}
export function CreateFoods(data) {
  return request({
    url: '/goodsservice/method/%xxx%usecase/CreateFoods',
    method: 'post',
    data
  })
}
export function DeleteFoods(data) {
  return request({
    url: '/goodsservice/method/%xxx%usecase/DeleteFoods',
    method: 'post',
    data
  })
}
export function UpdateFoods(data) {
  return request({
    url: '/goodsservice/method/%xxx%usecase/UpdateFoods',
    method: 'post',
    data
  })
}
