using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure.PersistenceObject
{
    public class User : Domain.Entities.User
    {
        public Guid AccountId { get; set; }
    }
}
