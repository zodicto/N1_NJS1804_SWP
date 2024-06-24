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

    public virtual DbSet<EducationalQualification> EducationalQualifications { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Rent> Rents { get; set; }

    public virtual DbSet<Rent1> Rent1s { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestLearning> RequestLearnings { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    public virtual DbSet<TutorSubject> TutorSubjects { get; set; }
    private string? GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DB_MiniCapStone"];
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC27C80CCB3C");

            entity.ToTable("Account");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.AccountBalance).HasColumnType("decimal(10, 0)");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Avatar)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_Of_Birth");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Roles).HasMaxLength(50);
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Class__3214EC27B12893DF");

            entity.ToTable("Class");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.ClassName).HasMaxLength(50);
        });

        modelBuilder.Entity<EducationalQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Educatio__3214EC27D7B934C9");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");
            entity.Property(e => e.Img)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.QualificationName).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.EducationalQualifications)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Education__ID_Tu__3E52440B");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC2726F94F34");

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
                .HasConstraintName("FK__RefreshTo__ID_Ac__38996AB5");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rent__3214EC27C7266FDA");

            entity.ToTable("Rent");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
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
            entity.Property(e => e.Price).HasColumnType("decimal(10, 0)");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Rent__ID_Account__5FB337D6");

            entity.HasOne(d => d.IdRequestNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdRequest)
                .HasConstraintName("FK__Rent__ID_Request__5DCAEF64");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdSubject)
                .HasConstraintName("FK__Rent__ID_Subject__5CD6CB2B");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Rent__ID_Tutor__5EBF139D");
        });

        modelBuilder.Entity<Rent1>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rent1__3214EC27D861ED55");

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
                .HasConstraintName("FK__Rent1__ID_Accoun__59FA5E80");

            entity.HasOne(d => d.IdScheduleNavigation).WithMany(p => p.Rent1s)
                .HasForeignKey(d => d.IdSchedule)
                .HasConstraintName("FK__Rent1__ID_Schedu__59063A47");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request__3214EC27999CE50F");

            entity.ToTable("Request");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
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
            entity.Property(e => e.Price).HasColumnType("decimal(10, 0)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Request__ID_Acco__48CFD27E");

            entity.HasOne(d => d.IdClassNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdClass)
                .HasConstraintName("FK__Request__ID_Clas__49C3F6B7");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdSubject)
                .HasConstraintName("FK__Request__ID_Subj__4AB81AF0");
        });

        modelBuilder.Entity<RequestLearning>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request___3214EC273B53529A");

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
                .HasConstraintName("FK__Request_L__ID_Re__4E88ABD4");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.RequestLearnings)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Request_L__ID_Tu__4D94879B");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC27D6BD569D");

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
                .HasConstraintName("FK__Schedule__ID_Req__5629CD9C");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdService)
                .HasConstraintName("FK__Schedule__ID_Ser__5535A963");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC27C71D6F46");

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
                .HasConstraintName("FK__Service__ID_Clas__52593CB8");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Service__ID_Tuto__5165187F");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3214EC276EBF9D9D");

            entity.ToTable("Subject");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.SubjectName).HasMaxLength(50);
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor__3214EC27F6740152");

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
                .HasConstraintName("FK__Tutor__ID_Accoun__3B75D760");
        });

        modelBuilder.Entity<TutorSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor_Su__3214EC277418878A");

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
                .HasConstraintName("FK__Tutor_Sub__ID_Su__440B1D61");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.TutorSubjects)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Tutor_Sub__ID_Tu__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
