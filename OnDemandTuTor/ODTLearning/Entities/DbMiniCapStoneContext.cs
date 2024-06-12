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

    public virtual DbSet<LearningModel> LearningModels { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Rent> Rents { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestLearning> RequestLearnings { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    public virtual DbSet<TutorSubject> TutorSubjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=12345;database=DBMiniCapstone;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC27F2EDBA20");

            entity.ToTable("Account");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
<<<<<<< HEAD
            entity.Property(e => e.AccountBalance).HasColumnType("decimal(10, 0)");
=======
            entity.Property(e => e.AccountBalance).HasColumnType("double");
>>>>>>> 5bf4fe10fc7aa035a5a3914b65e7f2704ccce55a
            entity.Property(e => e.Avatar)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Gmail).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<EducationalQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Educatio__3214EC276E07AB4E");

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
                .IsUnicode(false);
            entity.Property(e => e.QualificationName).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.EducationalQualifications)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Education__ID_Tu__0E04126B");
        });

        modelBuilder.Entity<LearningModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Learning__3214EC27C3F8270A");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.NameModel).HasMaxLength(50);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC27BC6A6C83");

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
                .HasConstraintName("FK__RefreshTo__ID_Ac__084B3915");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rent__3214EC274547CB5F");

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
                .HasConstraintName("FK__Rent__ID_Account__29AC2CE0");

            entity.HasOne(d => d.IdScheduleNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rent__ID_Schedul__28B808A7");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request__3214EC279B71CE84");

            entity.ToTable("Request");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.IdLearningModels)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_LearningModels");
            entity.Property(e => e.IdSubject)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Subject");
            entity.Property(e => e.LearningMethod).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 0)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request__ID_Acco__1881A0DE");

            entity.HasOne(d => d.IdLearningModelsNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdLearningModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request__ID_Lear__1975C517");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdSubject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request__ID_Subj__1A69E950");
        });

        modelBuilder.Entity<RequestLearning>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request___3214EC2738EB8D7C");

            entity.ToTable("Request_Learning");

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

            entity.HasOne(d => d.IdRequestNavigation).WithMany(p => p.RequestLearnings)
                .HasForeignKey(d => d.IdRequest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request_L__ID_Re__1E3A7A34");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.RequestLearnings)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Request_L__ID_Tu__1D4655FB");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC277E96BF74");

            entity.ToTable("Schedule");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
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
                .HasConstraintName("FK__Schedule__ID_Req__25DB9BFC");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedule__ID_Ser__24E777C3");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC2792A8E9D2");

            entity.ToTable("Service");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.IdLearningModels)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_LearningModels");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Video)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdLearningModelsNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdLearningModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__ID_Lear__220B0B18");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Service__ID_Tuto__2116E6DF");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3214EC2733832976");

            entity.ToTable("Subject");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.SubjectName).HasMaxLength(50);
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor__3214EC271873B976");

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
                .HasConstraintName("FK__Tutor__ID_Accoun__0B27A5C0");
        });

        modelBuilder.Entity<TutorSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor_Su__3214EC277EA45F32");

            entity.ToTable("Tutor_Subject");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdSubject)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Subject");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.TutorSubjects)
                .HasForeignKey(d => d.IdSubject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tutor_Sub__ID_Su__13BCEBC1");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.TutorSubjects)
                .HasForeignKey(d => d.IdTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tutor_Sub__ID_Tu__12C8C788");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
