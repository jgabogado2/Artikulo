using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Runtime.Intrinsics.X86;
using Artikulo.Models.DB;

namespace Artikulo.Models.DB
{
    public class MyDBContext : DbContext
    {
        public MyDBContext()
        {
        }
        public MyDBContext(DbContextOptions<MyDBContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<SystemUsers> SystemUsers { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Role> Role { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\ArtikuloDB; Initial Catalog=ArtikuloUsers; Integrated Security=True; Multiple Active Result Sets=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("SYSUserProfile");

                entity.Property(e => e.ProfileID)
                .HasColumnName("SYSUserProfileID")
                .HasColumnType("int");

                entity.Property(e => e.UserID)
                .HasColumnName("SYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(50)
                .IsUnicode(false);

                entity.Property(e => e.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(50)
                .IsUnicode(false);

                entity.Property(e => e.Email)
                .HasColumnName("Email")
                .HasMaxLength(150)
                .IsUnicode(false);

                entity.Property(e => e.Address)
                .HasColumnName("Address")
                .HasMaxLength(255)
                .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(12)
                .IsUnicode(false);

                entity.Property(e => e.Gender)
                .HasColumnName("Gender")
                .HasColumnType("char(1)");

                entity.Property(e => e.AccountImage)
                .HasColumnName("AccountImage")
                .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                .HasColumnName("RowCreatedSYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.CreatedDateTime)
                .HasColumnName("RowCreatedDateTime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifiedBy)
                .HasColumnName("RowModifiedSYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.ModifiedDateTime)
                .HasColumnName("RowModifiedDateTime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<SystemUsers>(entity =>
            {
                entity.ToTable("SYSUser");

                entity.Property(e => e.UserID)
                .HasColumnName("SYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.LoginName)
                .HasColumnName("LoginName")
                .HasMaxLength(50)
                .IsUnicode(false);

                entity.Property(e => e.PasswordEncryptedText)
                .HasColumnName("PasswordEncryptedText")
                .HasMaxLength(200)
                .IsUnicode(false);

                entity.Property(e => e.Salt)
                .HasColumnName("Salt")
                .HasMaxLength(128)
                .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                .HasColumnName("RowCreatedSYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.CreatedDateTime)
                .HasColumnName("RowCreatedDateTime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifiedBy)
                .HasColumnName("RowModifiedSYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.ModifiedDateTime)
                .HasColumnName("RowModifiedDateTime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("SYSUserRole");

                entity.HasKey(e => new { e.RoleID });

                entity.Property(e => e.RoleID)
                .HasColumnName("SYSUserRoleID")
                .HasColumnType("int");

                entity.Property(e => e.UserID)
                .HasColumnName("SYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.LookUpRoleID)
                .HasColumnName("LOOKUPRoleID")
                .HasColumnType("int");

                entity.Property(e => e.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("bit");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("RowCreatedSYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.CreatedDateTime)
                .HasColumnName("RowCreatedDateTime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifiedBy)
                .HasColumnName("RowModifiedSYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.ModifiedDateTime)
                .HasColumnName("RowModifiedDateTime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("LOOKUPRole");

                entity.Property(e => e.RoleID)
                .HasColumnName("LOOKUPRoleID")
                .HasColumnType("int");

                entity.Property(e => e.RoleName)
                .HasColumnName("RoleName")
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.RoleDescription)
                .HasColumnName("RoleDescription")
                .HasMaxLength(500)
                .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                .HasColumnName("RowCreatedSYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.CreatedDateTime)
                .HasColumnName("RowCreatedDateTime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ModifiedBy)
                .HasColumnName("RowModifiedSYSUserID")
                .HasColumnType("int");

                entity.Property(e => e.ModifiedDateTime)
                .HasColumnName("RowModifiedDateTime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
