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

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Complaint> Complaints { get; set; }

    public virtual DbSet<EducationalQualification> EducationalQualifications { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Rent> Rents { get; set; }

    public virtual DbSet<Rent1> Rent1s { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestLearning> RequestLearnings { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    public virtual DbSet<TutorSubject> TutorSubjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=12345;database=DBMiniCapstone;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC2749892C7C");

            entity.ToTable("Account");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_Of_Birth");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Roles).HasMaxLength(50);
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Class__3214EC270C572CBC");

            entity.ToTable("Class");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.ClassName).HasMaxLength(50);
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Complain__3214EC27275E7A6D");

            entity.ToTable("Complaint");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Complaint__ID_Ac__66603565");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Complaint__ID_Tu__6754599E");
        });

        modelBuilder.Entity<EducationalQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Educatio__3214EC27863478C6");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");
            entity.Property(e => e.QualificationName).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.EducationalQualifications)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Education__ID_Tu__68487DD7");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC274014B0AE");

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
                .HasConstraintName("FK__RefreshTo__ID_Ac__693CA210");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rent__3214EC270BFEEC6E");

            entity.ToTable("Rent");

            entity.HasIndex(e => e.IdRequest, "UQ__Rent__D5509881736BB20F").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.IdRequest)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Request");
            entity.Property(e => e.IdSubject)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Subject");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");

            entity.HasOne(d => d.IdRequestNavigation).WithOne(p => p.Rent)
                .HasForeignKey<Rent>(d => d.IdRequest)
                .HasConstraintName("FK__Rent__ID_Request__6A30C649");
        });

        modelBuilder.Entity<Rent1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rent1__3214EC2790F18C79");

            entity.ToTable("Rent1");

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

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Rent1s)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Rent1__ID_Accoun__6B24EA82");

            entity.HasOne(d => d.IdScheduleNavigation).WithMany(p => p.Rent1s)
                .HasForeignKey(d => d.IdSchedule)
                .HasConstraintName("FK__Rent1__ID_Schedu__6C190EBB");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request__3214EC27A77E27CD");

            entity.ToTable("Request");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.IdClass)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Class");
            entity.Property(e => e.IdSubject)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Subject");
            entity.Property(e => e.LearningMethod).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TimeTable).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Request__ID_Acco__6D0D32F4");

            entity.HasOne(d => d.IdClassNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdClass)
                .HasConstraintName("FK__Request__ID_Clas__6E01572D");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdSubject)
                .HasConstraintName("FK__Request__ID_Subj__6EF57B66");
        });

        modelBuilder.Entity<RequestLearning>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request___3214EC274AB12033");

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
                .HasConstraintName("FK__Request_L__ID_Re__6FE99F9F");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.RequestLearnings)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Request_L__ID_Tu__70DDC3D8");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC27AC97196B");

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
                .HasConstraintName("FK__Schedule__ID_Req__71D1E811");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdService)
                .HasConstraintName("FK__Schedule__ID_Ser__72C60C4A");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC277856923F");

            entity.ToTable("Service");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdClass)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Class");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Video)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdClassNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdClass)
                .HasConstraintName("FK__Service__ID_Clas__73BA3083");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Service__ID_Tuto__74AE54BC");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3214EC273DCADBF7");

            entity.ToTable("Subject");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.SubjectName).HasMaxLength(50);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC27B3F7E47C");

            entity.ToTable("Transaction");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Transacti__ID_Ac__787EE5A0");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor__3214EC27286E97F2");

            entity.ToTable("Tutor");

            entity.HasIndex(e => e.IdAccount, "UQ__Tutor__213379EA11312E4B").IsUnique();

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

            entity.HasOne(d => d.IdAccountNavigation).WithOne(p => p.Tutor)
                .HasForeignKey<Tutor>(d => d.IdAccount)
                .HasConstraintName("FK__Tutor__ID_Accoun__75A278F5");
        });

        modelBuilder.Entity<TutorSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor_Su__3214EC2749ADBC74");

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
                .HasConstraintName("FK__Tutor_Sub__ID_Su__76969D2E");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.TutorSubjects)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Tutor_Sub__ID_Tu__778AC167");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
