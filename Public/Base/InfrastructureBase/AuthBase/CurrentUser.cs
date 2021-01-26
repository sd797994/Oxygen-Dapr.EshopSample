using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase.AuthBase
{
    public class CurrentUser
    {
        public CurrentUser() { }
        public CurrentUser(Guid id, string loginName,string userImage, string nickName, int state, string userName, int? gender, DateTime? birthDay, string address, string tel, List<string> permissions)
        {
            Id = id;
            LoginName = loginName;
            UserImage = userImage;
            NickName = nickName;
            State = state;
            UserName = userName;
            Gender = gender;
            BirthDay = birthDay;
            Address = address;
            Tel = tel;
            if (permissions == null)
                IgnorePermission = true;
            else
                Permissions = permissions;
        }
        public Guid Id { get; set; }
        public string LoginName { get; set; }
        public string UserImage { get; set; }
        public string NickName { get; set; }
        public int State { get; set; }
        public string UserName { get; set; }
        public int? Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public List<string> Permissions { get; set; }
        public bool IgnorePermission { get; set; }
    }
}
