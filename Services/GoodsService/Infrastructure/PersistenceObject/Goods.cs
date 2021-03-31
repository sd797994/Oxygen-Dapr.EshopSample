using Infrastructure.EfDataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Infrastructure.PersistenceObject
{
    public partial class Goods : Domain.Entities.Goods
    {
        public DateTime? LastUpdateTime { get; set; }
    }
}
