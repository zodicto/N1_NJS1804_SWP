using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OrchidBusinessObjects;

namespace OrchidDAOs;

public partial class DbMiniCapStoneContext : DbContext
{
    public DbMiniCapStoneContext()
    {
    }

    public DbMiniCapStoneContext(DbContextOptions<DbMiniCapStoneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Degree> Degrees { get; set; }

    public virtual DbSet<Moderator> Moderators { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Rent> Rents { get; set; }

    public virtual DbSet<RequestTutor> RequestTutors { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    public virtual DbSet<TypeOfService> TypeOfServices { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;uid=sa;pwd=12345;database=DB_MiniCapStone;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3214EC27F2336988");

            entity.ToTable("Admin");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Admins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Admin__UserID__1F98B2C1");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC2780B5213D");

            entity.ToTable("Certificate");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CertificateName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Organization)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TutorId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TutorID");

            entity.HasOne(d => d.Tutor).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Certifica__Tutor__208CD6FA");
        });

        modelBuilder.Entity<Degree>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Degree__3214EC27A4C36A60");

            entity.ToTable("Degree");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Specialized)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TutorId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TutorID");

            entity.HasOne(d => d.Tutor).WithMany(p => p.Degrees)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Degree__TutorID__2180FB33");
        });

        modelBuilder.Entity<Moderator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Moderato__3214EC27DCAF69E0");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Moderators)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Moderator__UserI__22751F6C");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3214EC27B971648A");

            entity.ToTable("Post");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Price)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StudentID");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Student).WithMany(p => p.Posts)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Post__StudentID__236943A5");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rent__3214EC27360CF8A1");

            entity.ToTable("Rent");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.ServiceId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ServiceID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StudentId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("StudentID");

            entity.HasOne(d => d.Service).WithMany(p => p.Rents)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rent__ServiceID__02084FDA");

            entity.HasOne(d => d.Student).WithMany(p => p.Rents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rent__StudentID__02FC7413");
        });

        modelBuilder.Entity<RequestTutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RequestT__3214EC2755427F0F");

            entity.ToTable("RequestTutor");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.PostId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PostID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TutorId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TutorID");

            entity.HasOne(d => d.Post).WithMany(p => p.RequestTutors)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestTu__PostI__05D8E0BE");

            entity.HasOne(d => d.Tutor).WithMany(p => p.RequestTutors)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RequestTu__Tutor__06CD04F7");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC27AD2FA717");

            entity.ToTable("Role");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.NameRole)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC278D918D59");

            entity.ToTable("Service");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TutorId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TutorID");
            entity.Property(e => e.TypeOfServiceId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Type of serviceID");

            entity.HasOne(d => d.Tutor).WithMany(p => p.Services)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__TutorID__282DF8C2");

            entity.HasOne(d => d.TypeOfService).WithMany(p => p.Services)
                .HasForeignKey(d => d.TypeOfServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__Type of__29221CFB");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC27CDE86C8F");

            entity.ToTable("Student");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student__UserID__2A164134");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor__3214EC2762EF9566");

            entity.ToTable("Tutor");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.SpecializedSkills)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Tutors)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tutor__UserID__2B0A656D");
        });

        modelBuilder.Entity<TypeOfService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Type of __3214EC27395E1F83");

            entity.ToTable("Type of service");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.PostId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PostID");

            entity.HasOne(d => d.Post).WithMany(p => p.TypeOfServices)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Type of s__PostI__2BFE89A6");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC271F6380AE");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Birthdate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC0790A97CA7");

            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RoleID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__RoleI__2CF2ADDF");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__UserI__2DE6D218");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
