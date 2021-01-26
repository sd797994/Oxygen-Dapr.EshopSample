using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.EfDataAccess
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {

        }
        //Dbset<Po>
        public DbSet<PersistenceObject.%placeholder%> %placeholder% { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //启用Guid主键类型扩展
            modelBuilder.HasPostgresExtension("uuid-ossp");
            base.OnModelCreating(modelBuilder);
        }
    }
}
