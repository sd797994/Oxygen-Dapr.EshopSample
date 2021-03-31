using Domain.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Infrastructure.EfDataAccess
{
    public class EfDbContext : DbContext
    {
        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {

        }
        //Dbset<Po>
        public DbSet<PersistenceObject.Order> Order { get; set; }
        public DbSet<PersistenceObject.OrderItem> OrderItem { get; set; }
        public DbSet<PersistenceObject.TradeLog> TradeLog { get; set; }
        public DbSet<PersistenceObject.Logistics> Logistics { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //启用Guid主键类型扩展
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.Entity<PersistenceObject.OrderItem>().Property(x => x.GoodsSnapshot).HasConversion(x => JsonSerializer.Serialize(x, null), x => JsonSerializer.Deserialize<OrderGoodsSnapshot>(x, null));
            modelBuilder.Entity<OrderGoodsSnapshot>(builder => builder.HasNoKey());
            modelBuilder.Entity<PersistenceObject.Order>().Property(x => x.ConsigneeInfo).HasConversion(x => JsonSerializer.Serialize(x, null), x => JsonSerializer.Deserialize<OrderConsigneeInfo>(x, null));
            modelBuilder.Entity<OrderConsigneeInfo>(builder => builder.HasNoKey());
            base.OnModelCreating(modelBuilder);
        }
    }
}
