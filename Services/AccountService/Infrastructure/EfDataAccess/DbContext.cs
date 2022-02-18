using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.EfDataAccess
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {

        }
        public DbSet<PersistenceObject.Account> Account { get; set; }
        public DbSet<PersistenceObject.User> User { get; set; }
        public DbSet<PersistenceObject.Role> Role { get; set; }
        public DbSet<PersistenceObject.Permission> Permission { get; set; }
        public DbSet<PersistenceObject.UserRole> UserRole { get; set; }
        public DbSet<PersistenceObject.RolePermission> RolePermission { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //启用Guid主键类型扩展
            modelBuilder.HasPostgresExtension("uuid-ossp");
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnModelCreating(modelBuilder);
        }
    }
}
