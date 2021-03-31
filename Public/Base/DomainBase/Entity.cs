using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainBase
{
    /// <summary>
    /// 领域实体标记
    /// </summary>
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }
        /// <summary>
        /// key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
    }
}
