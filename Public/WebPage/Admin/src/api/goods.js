import request from '@/utils/request'

export function GetGoodsList(data) {
  return request({
    url: '/goodsservice/goodsquery/GetGoodsList',
    method: 'post',
    data
  })
}
export function CreateGoods(data) {
  return request({
    url: '/goodsservice/goodsusecase/CreateGoods',
    method: 'post',
    data
  })
}
export function DeleteGoods(data) {
  return request({
    url: '/goodsservice/goodsusecase/DeleteGoods',
    method: 'post',
    data
  })
}
export function UpdateGoods(data) {
  return request({
    url: '/goodsservice/goodsusecase/UpdateGoods',
    method: 'post',
    data
  })
}
export function GetAllCategoryList() {
  return request({
    url: '/goodsservice/categoryquery/GetAllCategoryList',
    method: 'post'
  })
}
export function UpOrDownShelfGoods(data) {
  return request({
    url: '/goodsservice/goodsusecase/UpOrDownShelfGoods',
    method: 'post',
    data
  })
}
export function SearchGoods(data) {
  return request({
    url: '/goodsservice/goodsquery/GetGoodslistByGoodsName',
    method: 'post',
    data
  })
}
export function UpdateGoodsStock(data) {
  return request({
    url: '/goodsservice/goodsusecase/UpdateGoodsStock',
    method: 'post',
    data
  })
}
