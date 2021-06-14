using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Protocols;

namespace BudgetWebApp.Models
{
    public partial class BudgetDatabaseContext : DbContext
    {
        public BudgetDatabaseContext()
        {
        }

        public BudgetDatabaseContext(DbContextOptions<BudgetDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BudgetItems> BudgetItems { get; set; }
        public virtual DbSet<Savings> Savings { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["BudgetDatabase"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BudgetItems>(entity =>
            {
                entity.HasKey(e => e.BudgetId)
                    .HasName("PK__BudgetIt__E38E7924EFAC6289");

                entity.Property(e => e.CarDeposit).HasColumnType("smallmoney");

                entity.Property(e => e.CarInsurancePrem).HasColumnType("smallmoney");

                entity.Property(e => e.CarModel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CarPrice).HasColumnType("smallmoney");

                entity.Property(e => e.Cellphone).HasColumnType("smallmoney");

                entity.Property(e => e.Groceries).HasColumnType("smallmoney");

                entity.Property(e => e.MonthlyIncome).HasColumnType("smallmoney");

                entity.Property(e => e.OtherExpense).HasColumnType("smallmoney");

                entity.Property(e => e.PropDeposit).HasColumnType("smallmoney");

                entity.Property(e => e.PropyPrice).HasColumnType("smallmoney");

                entity.Property(e => e.Rent).HasColumnType("smallmoney");

                entity.Property(e => e.TaxDeducted).HasColumnType("smallmoney");

                entity.Property(e => e.TravelCosts).HasColumnType("smallmoney");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WaterAndLight).HasColumnType("smallmoney");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.BudgetItems)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BudgetIte__Usern__1273C1CD");
            });

            modelBuilder.Entity<Savings>(entity =>
            {
                entity.HasKey(e => e.SavingId)
                    .HasName("PK__Savings__E3D384B9C5E37856");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TargetAmount).HasColumnType("smallmoney");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Savings)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Savings__Usernam__15502E78");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__Users__536C85E53E62FCE5");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
