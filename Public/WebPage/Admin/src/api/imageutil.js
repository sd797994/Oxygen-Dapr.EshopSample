import request from '@/utils/request'

export function UploadImage(data) {
  return request({
    url: '/imageservice/image/UploadByBase64',
    method: 'post',
    data
  })
}
