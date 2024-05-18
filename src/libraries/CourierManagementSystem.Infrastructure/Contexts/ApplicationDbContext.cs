using CourierManagementSystem.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourierManagementSystem.Infrastructure.Contexts;

//public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid,
//        UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IApplicationDbContext
public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
{
    //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    //    : base(options)
    //{
    //}


    private readonly string _connectionString;
    private readonly string _migrationAssemblyName;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    //public ApplicationDbContext(string connectionString, string migrationAssemblyName)
    //{
    //    _connectionString = connectionString;
    //    _migrationAssemblyName = migrationAssemblyName;
    //}

    //protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
    //{
    //    if (!dbContextOptionsBuilder.IsConfigured)
    //    {
    //        dbContextOptionsBuilder.UseSqlServer(
    //            _connectionString,
    //            m => m.MigrationsAssembly(_migrationAssemblyName));
    //    }

    //    base.OnConfiguring(dbContextOptionsBuilder);
    //}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var schemaName = "Booking";
        builder.Entity<BookParcel>().ToTable("BookParcels", schemaName);
        builder.Entity<Item>().ToTable("Items", schemaName);

        //ParcelOrder to Item one to many realtionship
        builder.Entity<BookParcel>()
            .HasMany<Item>(r => r.Items)
            .WithOne(p => p.ParcelOrder)
            .HasForeignKey(f => f.BookParcelId);

        base.OnModelCreating(builder);
        //builder.HasDefaultSchema("Identity");
        var schemaNameIdentity = "Identity";
        builder.Entity<IdentityUser>().ToTable("Users", schemaNameIdentity);
        builder.Entity<IdentityRole>().ToTable("Roles", schemaNameIdentity);
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", schemaNameIdentity);
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", schemaNameIdentity);
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", schemaNameIdentity);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", schemaNameIdentity);
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", schemaNameIdentity);

        //builder.Entity<IdentityUser>(entity =>
        //{
        //    entity.ToTable(name: "User");
        //});
        //builder.Entity<IdentityRole>(entity =>
        //{
        //    entity.ToTable(name: "Role");
        //});
        //builder.Entity<IdentityUserRole<string>>(entity =>
        //{
        //    entity.ToTable("UserRoles");
        //});
        //builder.Entity<IdentityUserClaim<string>>(entity =>
        //{
        //    entity.ToTable("UserClaims");
        //});
        //builder.Entity<IdentityUserLogin<string>>(entity =>
        //{
        //    entity.ToTable("UserLogins");
        //});
        //builder.Entity<IdentityRoleClaim<string>>(entity =>
        //{
        //    entity.ToTable("RoleClaims");
        //});
        //builder.Entity<IdentityUserToken<string>>(entity =>
        //{
        //    entity.ToTable("UserTokens");
        //});
    }
}
