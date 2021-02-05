using InfrastructureBase.AuthBase;
using Oxygen.Client.ServerProxyFactory.Interface;
using Oxygen.Client.ServerSymbol.Store;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class PermissionListCacheHelper
    {
        static SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        public static async Task GroupAndSave(IStateManager stateManager, List<AuthenticationInfo> data)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (data.Any())
                {
                    var remoteData = await stateManager.GetState<List<AuthenticationInfo>>(new PermissionListCacheStore());
                    if (remoteData == null)
                        remoteData = new List<AuthenticationInfo>();
                    remoteData.AddRange(data);
                    var distinctData = remoteData.Where(x => x.CheckPermission).GroupBy(x => x.Path).Select(y => new AuthenticationInfo(y.FirstOrDefault().SrvName, y.FirstOrDefault().FuncName, y.FirstOrDefault().CheckPermission, y.Key)).ToList();
                    await stateManager.SetState(new PermissionListCacheStore(distinctData));
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
    public class PermissionListCacheStore : StateStore
    {
        public PermissionListCacheStore(List<AuthenticationInfo> data)
        {
            Key = $"PermissionListCacheStore";
            this.Data = data;
        }
        public PermissionListCacheStore()
        {
            Key = $"PermissionListCacheStore";
        }
        public override string Key { get; set; }
        public override object Data { get; set; }
    }
}
