using Microsoft.EntityFrameworkCore;

namespace TaskApi.Data.Entities;

public partial class TasksDbContext(DbContextOptions<TasksDbContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");
            entity.ToTable("user");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(50).HasColumnName("email");
            entity.Property(e => e.Password).HasMaxLength(255).HasColumnName("password");
            entity.Property(e => e.PasswordSalt).HasMaxLength(255).HasColumnName("password_salt");
            entity.Property(e => e.FirstName).HasMaxLength(255).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasMaxLength(255).HasColumnName("last_name");
            entity.Property(e => e.Ts).HasColumnType("timestamp without time zone").HasColumnName("ts");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
        });
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_token_pkey");
            entity.ToTable("refresh_token");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TokenHash).HasMaxLength(1000).HasColumnName("token_hash");
            entity.Property(e => e.TokenSalt).HasMaxLength(50).HasColumnName("token_salt");
            entity.Property(e => e.Ts).HasColumnType("timestamp without time zone").HasColumnName("ts");
            entity.Property(e => e.ExpiryDate).HasColumnType("timestamp without time zone").HasColumnName("expiry_date");
            entity.HasOne(e => e.User).WithMany(e => e.RefreshTokens)
                .HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("refresh_token_user_id_fkey");
        });
        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");
            entity.ToTable("task");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.IsCompleted).HasColumnName("is_completed");
            entity.Property(e => e.Ts).HasColumnType("timestamp without time zone").HasColumnName("ts");
            entity.HasOne(e => e.User).WithMany(e => e.Tasks)
                .HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_user_id_fkey");
        });
    }
}
