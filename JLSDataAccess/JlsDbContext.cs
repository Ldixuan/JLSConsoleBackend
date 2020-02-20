using JLSDataModel.Models;
using JLSDataModel.Models.Adress;
using JLSDataModel.Models.Audit;
using JLSDataModel.Models.Order;
using JLSDataModel.Models.Product;
using JLSDataModel.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JLSDataAccess
{
    public class JlsDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public JlsDbContext(DbContextOptions<JlsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ReferenceCategory> ReferenceCategory { get; set; }

        public virtual DbSet<ReferenceItem> ReferenceItem { get; set; }

        public virtual DbSet<ReferenceLabel> ReferenceLabel { get; set; }

        public virtual DbSet<OrderInfo> OrderInfo { get; set; }
        public virtual DbSet<OrderInfoLog> OrderInfoLog { get; set; }


        public virtual DbSet<DiscountActivity> DiscountActivity { get; set; }

        public virtual DbSet<DiscountActivityProduct> DiscountActivityProduct { get; set; }

        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<ProductPhotoPath> ProductPhotoPath { get; set; }
        public virtual DbSet<UserPreferenceCategory> UserPreferenceCategory { get; set; }

        public virtual DbSet<Adress> Adress { get; set; }

        public virtual DbSet<UserToken> UserToken { get; set; }
        public virtual DbSet<UserShippingAdress> UserShippingAdress { get; set; }

        public virtual DbSet<Audit> Audit { get; set; }
        public virtual DbSet<AuditData> AuditData { get; set; }

        public virtual DbSet<DeletedRecords> DeletedRecords { get; set; }

    }
}
