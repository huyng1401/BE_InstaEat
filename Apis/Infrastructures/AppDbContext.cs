using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructures
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Package> Packages { get; set; }

        public virtual DbSet<Restaurant> Restaurants { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF35B3AF54");

                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.HasOne(d => d.Package).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK__Order__PackageID__403A8C7D");

                entity.HasOne(d => d.Restaurant).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RestaurantId)
                    .HasConstraintName("FK__Order__Restauran__3F466844");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.PackageId).HasName("PK__Package__322035EC47C6730A");

                entity.ToTable("Package");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.PackageName).HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.HasKey(e => e.RestaurantId).HasName("PK__Restaura__87454CB5BD880A8D");

                entity.ToTable("Restaurant");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.Image).HasMaxLength(255);
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.RestaurantName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User).WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Restauran__UserI__2C3393D0");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79AEF3BAB454");

                entity.ToTable("Review");

                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
                entity.Property(e => e.Created).HasColumnType("datetime");
                entity.Property(e => e.Image).HasMaxLength(255);
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
                entity.Property(e => e.Status).HasDefaultValueSql("((0))");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Restaurant).WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.RestaurantId)
                    .HasConstraintName("FK__Review__Restaura__31EC6D26");

                entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Review__UserID__30F848ED");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3AA5CF4C50");

                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B41820DE4");

                entity.ToTable("Transaction");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
                entity.Property(e => e.Created).HasColumnType("datetime");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Restaurant).WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.RestaurantId)
                    .HasConstraintName("FK__Transacti__Resta__36B12243");

                entity.HasOne(d => d.Review).WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ReviewId)
                    .HasConstraintName("FK__Transacti__Revie__38996AB5");

                entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Transacti__UserI__37A5467C");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACE2481AE3");

                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User__RoleID__286302EC");
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.WalletId).HasName("PK__Wallet__84D4F92ED43DDBF7");

                entity.ToTable("Wallet");

                entity.Property(e => e.WalletId).HasColumnName("WalletID");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.TotalPoint)
                    .HasDefaultValueSql("((0))")
                    .HasColumnType("decimal(10, 2)");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Wallet__UserID__44FF419A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
