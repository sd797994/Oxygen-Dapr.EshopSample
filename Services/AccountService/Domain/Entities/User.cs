using DomainBase;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    /// <summary>
    /// 用户领域
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImage { get; set; }
        /// <summary>
        /// 用户性别
        /// </summary>
        public UserGender Gender { get; set; }
        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime? BirthDay { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 创建或修改用户基本信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="gender"></param>
        /// <param name="birthDay"></param>
        public void CreateOrUpdateUser(string userName, string userImage, string address, string tel, UserGender gender, DateTime? birthDay)
        {
            if (!string.IsNullOrEmpty(userName))
                UserName = userName;
            if (!string.IsNullOrEmpty(userImage))
                UserImage = userImage;
            Gender = gender;
            if (!string.IsNullOrEmpty(address))
                Address = address;
            if (!string.IsNullOrEmpty(tel))
                Tel = tel;
            if (birthDay != null && birthDay >= DateTime.Now.Date)
                throw new DomainException("生日不能大于当前日期");
            BirthDay = birthDay;
        }
    }
}
