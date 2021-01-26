import request from '@/utils/request'

export function GetGoodsList(data) {
  return request({
    url: '/goodsservice/method/goodsquery/GetGoodsList',
    method: 'post',
    data
  })
}
export function CreateGoods(data) {
  return request({
    url: '/goodsservice/method/goodsusecase/CreateGoods',
    method: 'post',
    data
  })
}
export function DeleteGoods(data) {
  return request({
    url: '/goodsservice/method/goodsusecase/DeleteGoods',
    method: 'post',
    data
  })
}
export function UpdateGoods(data) {
  return request({
    url: '/goodsservice/method/goodsusecase/UpdateGoods',
    method: 'post',
    data
  })
}
export function GetAllCategoryList() {
  return request({
    url: '/goodsservice/method/categoryquery/GetAllCategoryList',
    method: 'post'
  })
}
export function UpOrDownShelfGoods(data) {
  return request({
    url: '/goodsservice/method/goodsusecase/UpOrDownShelfGoods',
    method: 'post',
    data
  })
}
export function SearchGoods(data) {
  return request({
    url: '/goodsservice/method/goodsquery/GetGoodslistByGoodsName',
    method: 'post',
    data
  })
}
