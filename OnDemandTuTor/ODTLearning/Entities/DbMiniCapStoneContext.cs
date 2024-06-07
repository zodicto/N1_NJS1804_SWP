using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ODTLearning.Entities;

public partial class DbminiCapstoneContext : DbContext
{
    public DbminiCapstoneContext()
    {
    }

    public DbminiCapstoneContext(DbContextOptions<DbminiCapstoneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<EducationalQualification> EducationalQualifications { get; set; }

    public virtual DbSet<Field> Fields { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Rent> Rents { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<ResquestLearning> ResquestLearnings { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    public virtual DbSet<TutorField> TutorFields { get; set; }

    public virtual DbSet<TypeOfService> TypeOfServices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=DBMiniCapstone;User ID=sa;Password=12345;Trusted_Connection=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC27638F4951");

            entity.ToTable("Account");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Gmail).HasMaxLength(50);
            entity.Property(e => e.Img)
                .HasMaxLength(50)
                .HasColumnName("img");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<EducationalQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Educatio__3214EC27D68BC1C7");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");
            entity.Property(e => e.Img)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.QualificationName).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.EducationalQualifications)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Education__ID_Tu__45BE5BA9");
        });

        modelBuilder.Entity<Field>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Field__3214EC27A00BD2E9");

            entity.ToTable("Field");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.FieldName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC27A8773FF1");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.IssuedAt).HasColumnType("datetime");
            entity.Property(e => e.JwtId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.IdAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__ID_Ac__40058253");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rent__3214EC274E944030");

            entity.ToTable("Rent");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.IdSchedule)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Schedule");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rent__ID_Account__607251E5");

            entity.HasOne(d => d.IdScheduleNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rent__ID_Schedul__5F7E2DAC");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request__3214EC277F9240A0");

            entity.ToTable("Request");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.IdTypeOfService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_TypeOfService");
            entity.Property(e => e.Price).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Titile).HasMaxLength(50);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request__ID_Acco__503BEA1C");

            entity.HasOne(d => d.IdTypeOfServiceNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdTypeOfService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request__ID_Type__51300E55");
        });

        modelBuilder.Entity<ResquestLearning>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Resquest__3214EC2719A9E153");

            entity.ToTable("Resquest_Learning");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdRequest)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Request");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");

            entity.HasOne(d => d.IdRequestNavigation).WithMany(p => p.ResquestLearnings)
                .HasForeignKey(d => d.IdRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resquest___ID_Re__55009F39");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.ResquestLearnings)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resquest___ID_Tu__540C7B00");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC27664BA4E3");

            entity.ToTable("Schedule");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnName("DATE");
            entity.Property(e => e.IdRequest)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Request");
            entity.Property(e => e.IdService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Service");

            entity.HasOne(d => d.IdRequestNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__ID_Req__5BAD9CC8");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__ID_Ser__5CA1C101");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC27FD95DBF9");

            entity.ToTable("Service");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");
            entity.Property(e => e.IdTypeOfService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Id_TypeOfService");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Video)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__ID_Tuto__57DD0BE4");

            entity.HasOne(d => d.IdTypeOfServiceNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdTypeOfService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__Id_Type__58D1301D");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor__3214EC27A28747AF");

            entity.ToTable("Tutor");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.SpecializedSkills).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Tutors)
                .HasForeignKey(d => d.IdAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tutor__ID_Accoun__42E1EEFE");
        });

        modelBuilder.Entity<TutorField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor_Fi__3214EC277D035C5A");

            entity.ToTable("Tutor_Field");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdField)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Field");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");

            entity.HasOne(d => d.IdFieldNavigation).WithMany(p => p.TutorFields)
                .HasForeignKey(d => d.IdField)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tutor_Fie__ID_Fi__4B7734FF");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.TutorFields)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tutor_Fie__ID_Tu__4A8310C6");
        });

        modelBuilder.Entity<TypeOfService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypeOfSe__3214EC273EAB3FEA");

            entity.ToTable("TypeOfService");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.NameService).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
