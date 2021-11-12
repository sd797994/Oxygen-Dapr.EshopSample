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
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            modelBuilder.Entity<PersistenceObject.OrderItem>().Property(x => x.GoodsSnapshot).HasConversion(x => TmpJosnSerializer<OrderGoodsSnapshot>.Serialize(x), x => TmpJosnSerializer<OrderGoodsSnapshot>.Deserialize(x));
            modelBuilder.Entity<OrderGoodsSnapshot>(builder => builder.HasNoKey());
            modelBuilder.Entity<PersistenceObject.Order>().Property(x => x.ConsigneeInfo).HasConversion(x => TmpJosnSerializer<OrderConsigneeInfo>.Serialize(x), x => TmpJosnSerializer<OrderConsigneeInfo>.Deserialize(x));
            modelBuilder.Entity<OrderConsigneeInfo>(builder => builder.HasNoKey());
            base.OnModelCreating(modelBuilder);
        }
        class TmpJosnSerializer<T>
        {
            public static Func<T, string> Serialize = (x) => JsonSerializer.Serialize(x);
            public static Func<string, T> Deserialize = (x) => JsonSerializer.Deserialize<T>(x);
        }
    }
}
