using Oxygen.Client.ServerSymbol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.Base
{
    [RemoteService("imageservice", "image", "图片服务")]
    public interface IImageAppService
    {
        [RemoteFunc(funcDescription: "上传图片")]
        Task<ApiResult> UploadByBase64(UploadImageDto input);
    }
    public class UploadImageDto
    {
        public string Base64Body { get; set; }
    }
}
