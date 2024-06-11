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

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Package> Packages { get; set; }

        public virtual DbSet<Restaurant> Restaurants { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA586320FEF8C");

                entity.ToTable("Account");

                entity.HasIndex(e => e.Username, "UQ__Account__536C85E4DB4B9C2B").IsUnique();

                entity.Property(e => e.AccountId).HasColumnName("AccountID");
                entity.Property(e => e.AssociatedId).HasColumnName("AssociatedID");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.TotalPoint).HasDefaultValueSql("((0))");
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Associated).WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AssociatedId)
                    .HasConstraintName("FK_AssociatedID_Customer");

                entity.HasOne(d => d.AssociatedNavigation).WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.AssociatedId)
                    .HasConstraintName("FK_AssociatedID_Restaurant");

                entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B806E47454");

                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF4562D0E1");

                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.OrderDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.HasOne(d => d.Package).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK__Order__PackageID__3429BB53");

                entity.HasOne(d => d.Restaurant).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RestaurantId)
                    .HasConstraintName("FK__Order__Restauran__3335971A");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.PackageId).HasName("PK__Package__322035EC495CFEAE");

                entity.ToTable("Package");

                entity.Property(e => e.PackageId).HasColumnName("PackageID");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.PackageName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.HasKey(e => e.RestaurantId).HasName("PK__Restaura__87454CB5887D9F0E");

                entity.ToTable("Restaurant");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.Image).IsUnicode(false);
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.RestaurantName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79AEC4E63FE0");

                entity.ToTable("Review");

                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.Image).IsUnicode(false);
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Review__Customer__2D7CBDC4");

                entity.HasOne(d => d.Restaurant).WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.RestaurantId)
                    .HasConstraintName("FK__Review__Restaura__2E70E1FD");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3A9B370FEF");

                entity.ToTable("Role");

                entity.HasIndex(e => e.RoleName, "UQ__Role__8A2B61608557C3F0").IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("RoleID");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4BE545F9BB");

                entity.ToTable("Transaction");

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");
                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");

                entity.HasOne(d => d.Customer).WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Transacti__Custo__39E294A9");

                entity.HasOne(d => d.Restaurant).WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.RestaurantId)
                    .HasConstraintName("FK__Transacti__Resta__38EE7070");

                entity.HasOne(d => d.Review).WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ReviewId)
                    .HasConstraintName("FK__Transacti__Revie__3AD6B8E2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}
