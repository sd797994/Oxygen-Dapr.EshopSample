using IApplicationService;
using IApplicationService.Base;
using InfrastructureBase;
using InfrastructureBase.AuthBase;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageService.ImageAppService
{
    public class ImageAppService : IImageAppService
    {
        [AuthenticationFilter(false)]
        public async Task<ApiResult> UploadByBase64(UploadImageDto input)
        {
            if (!string.IsNullOrEmpty(input.Base64Body))
            {
                try
                {
                    var dir = $"{DateTime.Now:yyyyMM}";
                    var imageurl =$"{dir}/{Guid.NewGuid()}.jpg";
                    byte[] imageBytes = Convert.FromBase64String(input.Base64Body.Replace("data:image/jpeg;base64,", ""));
                    Directory.CreateDirectory($"wwwroot/{dir}");
                    using var ms = new FileStream($"wwwroot/{imageurl}", FileMode.Create);
                    await ms.WriteAsync(imageBytes, 0, imageBytes.Length);
                    return ApiResult.Ok(imageurl, "");
                }
                catch (Exception)
                {
                    return ApiResult.Err();
                }
            }
            else
                throw new ApplicationServiceException("无法识别的图片");
        }
    }
}
