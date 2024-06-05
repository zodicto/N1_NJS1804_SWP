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

    public virtual DbSet<Acount> Acounts { get; set; }

    public virtual DbSet<EducationalQualification> EducationalQualifications { get; set; }

    public virtual DbSet<Field> Fields { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Rent> Rents { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<ResquestLearning> ResquestLearnings { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    public virtual DbSet<TutorField> TutorFields { get; set; }

    public virtual DbSet<TypeOfService> TypeOfServices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=DBMiniCapstone;User ID=sa;Password=12345;Trusted_Connection=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acount>(entity =>
        {
            entity.HasKey(e => e.IdAccount).HasName("PK__Acount__ADA956219F259ECE");

            entity.ToTable("Acount");

            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Account");
            entity.Property(e => e.Birthdate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Gmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EducationalQualification>(entity =>
        {
            entity.HasKey(e => e.IdEducationalEualifications).HasName("PK__Educatio__C1B293B0B7005E91");

            entity.Property(e => e.IdEducationalEualifications)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_EducationalEualifications");
            entity.Property(e => e.CertificateName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Tutor");
            entity.Property(e => e.Img)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.EducationalQualifications)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKEducationa811691");
        });

        modelBuilder.Entity<Field>(entity =>
        {
            entity.HasKey(e => e.IdField).HasName("PK__Field__0CC4080769EE2C69");

            entity.ToTable("Field");

            entity.Property(e => e.IdField)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Field");
            entity.Property(e => e.FieldName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07E68F9E08");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Account");
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
                .HasConstraintName("FKRefreshTok940738");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.IdRent).HasName("PK__Rent__478EEDDF1112658C");

            entity.ToTable("Rent");

            entity.Property(e => e.IdRent)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Rent");
            entity.Property(e => e.IdSchedule)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Schedule");
            entity.Property(e => e.IdStudent)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id_Student");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdScheduleNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKRent911520");

            entity.HasOne(d => d.IdStudentNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdStudent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKRent745294");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.IdPost).HasName("PK__Request__2BA425F7AD45E84E");

            entity.ToTable("Request");

            entity.Property(e => e.IdPost)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Post");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IdStudent)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id_Student");
            entity.Property(e => e.IdTypeOfService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_TypeOfService");
            entity.Property(e => e.Price)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Titile)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdStudentNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdStudent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKRequest420764");

            entity.HasOne(d => d.IdTypeOfServiceNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdTypeOfService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKRequest882743");
        });

        modelBuilder.Entity<ResquestLearning>(entity =>
        {
            entity.HasKey(e => e.IdRequestLearning).HasName("PK__Resquest__9ED0AE2FD2293FB4");

            entity.ToTable("Resquest_Learning");

            entity.Property(e => e.IdRequestLearning)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_RequestLearning");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Tutor");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TidPost)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tid_Post");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.ResquestLearnings)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKResquest_L23721");

            entity.HasOne(d => d.TidPostNavigation).WithMany(p => p.ResquestLearnings)
                .HasForeignKey(d => d.TidPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKResquest_L231869");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.IdSchedule).HasName("PK__Schedule__B70A1A11803E1230");

            entity.ToTable("Schedule");

            entity.Property(e => e.IdSchedule)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Schedule");
            entity.Property(e => e.IdPost)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Post");
            entity.Property(e => e.IdService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Service");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdPost)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKSchedule601757");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKSchedule81136");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("PK__Service__F6F54EA7AA2410E3");

            entity.ToTable("Service");

            entity.Property(e => e.IdService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Service");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdTypeOfService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_TypeOfService");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TutoridTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tutorid_Tutor");
            entity.Property(e => e.Video)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTypeOfServiceNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdTypeOfService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKService343958");

            entity.HasOne(d => d.TutoridTutorNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.TutoridTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKService741934");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.IdStudent).HasName("PK__Student__0098208F9F528844");

            entity.ToTable("Student");

            entity.Property(e => e.IdStudent)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("id_Student");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Account");
            entity.Property(e => e.Img)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("img");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.IdAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKStudent366114");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.IdTutor).HasName("PK__Tutor__93DA661D67525E70");

            entity.ToTable("Tutor");

            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Tutor");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Account");
            entity.Property(e => e.SpecializedSkills)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Tutors)
                .HasForeignKey(d => d.IdAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKTutor303168");
        });

        modelBuilder.Entity<TutorField>(entity =>
        {
            entity.HasKey(e => e.IdTutorFileld).HasName("PK__Tutor_Fi__CBB4B60C7457DC2B");

            entity.ToTable("Tutor_Field");

            entity.Property(e => e.IdTutorFileld)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Tutor_Fileld");
            entity.Property(e => e.IdField)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Field");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Tutor");

            entity.HasOne(d => d.IdFieldNavigation).WithMany(p => p.TutorFields)
                .HasForeignKey(d => d.IdField)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKTutor_Fiel99285");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.TutorFields)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKTutor_Fiel195321");
        });

        modelBuilder.Entity<TypeOfService>(entity =>
        {
            entity.HasKey(e => e.IdTypeOfService).HasName("PK__Type of __0DF9946F0D2A9F73");

            entity.ToTable("Type of service");

            entity.Property(e => e.IdTypeOfService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_TypeOfService");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
