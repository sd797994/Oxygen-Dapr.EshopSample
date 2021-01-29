import request from '@/utils/request'

export function GetCategoryList(data) {
  return request({
    url: '/goodsservice/categoryquery/GetCategoryList',
    method: 'post',
    data
  })
}
export function CreateCategory(data) {
  return request({
    url: '/goodsservice/categoryusecase/CreateCategory',
    method: 'post',
    data
  })
}
export function DeleteCategory(data) {
  return request({
    url: '/goodsservice/categoryusecase/DeleteCategory',
    method: 'post',
    data
  })
}
export function UpdateCategory(data) {
  return request({
    url: '/goodsservice/categoryusecase/UpdateCategory',
    method: 'post',
    data
  })
}
