import request from '@/utils/request'

export function GetCategoryList(data) {
  return request({
    url: '/goodsservice/method/categoryquery/GetCategoryList',
    method: 'post',
    data
  })
}
export function CreateCategory(data) {
  return request({
    url: '/goodsservice/method/categoryusecase/CreateCategory',
    method: 'post',
    data
  })
}
export function DeleteCategory(data) {
  return request({
    url: '/goodsservice/method/categoryusecase/DeleteCategory',
    method: 'post',
    data
  })
}
export function UpdateCategory(data) {
  return request({
    url: '/goodsservice/method/categoryusecase/UpdateCategory',
    method: 'post',
    data
  })
}
