using Domain.Dtos;
using Domain.Repository;
using DomainBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class PermissionMultiCreateService
    {
        private readonly IPermissionRepository repository;
        private readonly IEnumerable<CreatePermissionTmpDto> input;
        public PermissionMultiCreateService(IPermissionRepository repository, IEnumerable<CreatePermissionTmpDto> input)
        {
            this.repository = repository;
            this.input = input;
        }
        public void Create()
        {
            if (input.Any())
            {
                var domain = new List<Permission>();
                var allFatherPermissions = input.GroupBy(x => x.ServerName).Select(x => x.Key).ToList();
                if (!allFatherPermissions.Any())
                    throw new DomainException("服务名无效!");
                repository.Delete(x => true);//直接删除所有
                allFatherPermissions.ForEach(x =>
                {
                    var permission = new Permission();
                    permission.CreatePermission(Guid.Empty, x, "");
                    repository.Add(permission);
                    var child = input.Where(y => y.ServerName == x).ToList();
                    if (!child.Any())
                    {
                        throw new DomainException($"服务{x}下没有有效接口!");
                    }
                    else
                    {
                        child.ForEach(y =>
                        {
                            if(string.IsNullOrEmpty(y.Path))
                                throw new DomainException($"服务{x}下没有有效接口地址!");
                            var childpermission = new Permission();
                            childpermission.CreatePermission(permission.Id, y.PermissionName, y.Path);
                            repository.Add(childpermission);
                        });
                    }
                });
            }
            else
            {
                throw new DomainException("请至少输入一个权限");
            }
        }
    }
}
