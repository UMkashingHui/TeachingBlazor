using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeachingWebApi.Models.Identity;

namespace TeachingWebApi.Utils.Data
{
    public class MongoIdentityDbContext : DbContext
    {

        public MongoIdentityDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.Name is "LastModifiedBy" or "CreatedBy"))
            {
                property.SetColumnType("nvarchar(128)");
            }

            base.OnModelCreating(builder);
            builder.Entity<TeachingBlazorUser>(entity =>
            {
                entity.ToTable(name: "Users", "Identity");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<TeachingBlazorRole>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity").HasKey(x => x.Id);
            });

            builder.Entity<IdentityUserClaim<ObjectId>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

        }
    }
}
