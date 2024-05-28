using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OTDLeaningTest.Entities;

namespace OTDLeaningTest.DataAccessLayer;

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

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Field> Fields { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Rent> Rents { get; set; }

    public virtual DbSet<ResquestTutor> ResquestTutors { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    public virtual DbSet<TutorField> TutorFields { get; set; }

    public virtual DbSet<TypeOfService> TypeOfServices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=DBMiniCapstone;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.IdAccount).HasName("PK__Account__ADA956212E0C4E76");

            entity.ToTable("Account");

            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Account");
            entity.Property(e => e.Birthdate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FisrtName)
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
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EducationalQualification>(entity =>
        {
            entity.HasKey(e => e.IdEducationalEualifications).HasName("PK__Educatio__C1B293B0C50E68E8");

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
            entity.Property(e => e.Organization)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.EducationalQualifications)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Education__id_Tu__324172E1");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.IdFeedback).HasName("PK__Feedback__36BC863086749877");

            entity.ToTable("Feedback");

            entity.HasIndex(e => e.IdService, "UQ__Feedback__F6F54EA6E556053E").IsUnique();

            entity.Property(e => e.IdFeedback)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_feedback");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Account");
            entity.Property(e => e.IdService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Service");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Feedback__id_Acc__3DB3258D");
        });

        modelBuilder.Entity<Field>(entity =>
        {
            entity.HasKey(e => e.IdField).HasName("PK__Field__0CC408078A47A724");

            entity.ToTable("Field");

            entity.Property(e => e.IdField)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Field");
            entity.Property(e => e.FieldName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.IdChat).HasName("PK__Messages__316B3F44B536C3B4");

            entity.Property(e => e.IdChat)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Chat");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Account");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Messages__id_Acc__54968AE5");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.IdPost).HasName("PK__Post__2BA425F7AC64A41D");

            entity.ToTable("Post");

            entity.Property(e => e.IdPost)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Post");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Account");
            entity.Property(e => e.IdTypeOfService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_TypeOfService");
            entity.Property(e => e.Price)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Titile)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Post__id_Account__4924D839");

            entity.HasOne(d => d.IdTypeOfServiceNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.IdTypeOfService)
                .HasConstraintName("FK__Post__id_TypeOfS__4A18FC72");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC0750897F01");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.Id).ValueGeneratedNever();
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
                .HasConstraintName("FK__RefreshTo__ID_Ac__5A4F643B");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.IdRent).HasName("PK__Rent__478EEDDFA4D65269");

            entity.ToTable("Rent");

            entity.Property(e => e.IdRent)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Rent");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Account");
            entity.Property(e => e.IdSchedule)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Schedule");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Rent__id_Account__4CF5691D");

            entity.HasOne(d => d.IdScheduleNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdSchedule)
                .HasConstraintName("FK__Rent__id_Schedul__4DE98D56");
        });

        modelBuilder.Entity<ResquestTutor>(entity =>
        {
            entity.HasKey(e => e.IdRequestTutor).HasName("PK__Resquest__61285A5E8EB2641D");

            entity.ToTable("ResquestTutor");

            entity.Property(e => e.IdRequestTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_RequestTutor");
            entity.Property(e => e.IdPost)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Post");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Tutor");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.ResquestTutors)
                .HasForeignKey(d => d.IdPost)
                .HasConstraintName("FK__ResquestT__id_Po__51BA1E3A");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.ResquestTutors)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__ResquestT__id_Tu__50C5FA01");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.IdSchedule).HasName("PK__Schedule__B70A1A1150945D1A");

            entity.ToTable("Schedule");

            entity.Property(e => e.IdSchedule)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Schedule");
            entity.Property(e => e.IdService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Service");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Tutor");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdService)
                .HasConstraintName("FK__Schedule__id_Ser__46486B8E");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Schedule__id_Tut__45544755");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("PK__Service__F6F54EA7038DCAB7");

            entity.ToTable("Service");

            entity.HasIndex(e => e.IdFeedback, "UQ__Service__36BC8631BD63F430").IsUnique();

            entity.Property(e => e.IdService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_Service");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdFeedback)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_feedback");
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

            entity.HasOne(d => d.IdFeedbackNavigation).WithOne(p => p.Service)
                .HasForeignKey<Service>(d => d.IdFeedback)
                .HasConstraintName("FK__Service__id_feed__4277DAAA");

            entity.HasOne(d => d.IdTypeOfServiceNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdTypeOfService)
                .HasConstraintName("FK__Service__id_Type__4183B671");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.IdTutor).HasName("PK__Tutor__93DA661D916EF89E");

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
                .HasConstraintName("FK__Tutor__id_Accoun__2F650636");
        });

        modelBuilder.Entity<TutorField>(entity =>
        {
            entity.HasKey(e => e.IdTutorFileld).HasName("PK__Tutor_Fi__CBB4B60C26DD204F");

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
                .HasConstraintName("FK__Tutor_Fie__id_Fi__37FA4C37");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.TutorFields)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Tutor_Fie__id_Tu__370627FE");
        });

        modelBuilder.Entity<TypeOfService>(entity =>
        {
            entity.HasKey(e => e.IdTypeOfService).HasName("PK__Type of __0DF9946FAF2D937A");

            entity.ToTable("Type of service");

            entity.Property(e => e.IdTypeOfService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_TypeOfService");
            entity.Property(e => e.NameService)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
