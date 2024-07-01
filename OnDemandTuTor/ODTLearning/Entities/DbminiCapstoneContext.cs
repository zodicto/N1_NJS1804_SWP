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

    public virtual DbSet<Available> Availables { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Complaint> Complaints { get; set; }

    public virtual DbSet<EducationalQualification> EducationalQualifications { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Rent> Rents { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestLearning> RequestLearnings { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<TimeSlot> TimeSlots { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC2739E9DFF9");

            entity.ToTable("Account");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_Of_Birth");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Roles).HasMaxLength(50);
        });

        modelBuilder.Entity<Available>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Availabl__3214EC27EC2B8455");

            entity.ToTable("Available");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdTimeSlot)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_TimeSlot");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");

            entity.HasOne(d => d.IdTimeSlotNavigation).WithMany(p => p.Availables)
                .HasForeignKey(d => d.IdTimeSlot)
                .HasConstraintName("FK__Available__ID_Ti__778AC167");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Availables)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Available__ID_Tu__76969D2E");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC27F7261565");

            entity.ToTable("Booking");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.IdAvailable)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Available");
            entity.Property(e => e.IdService)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Service");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Booking__ID_Acco__7A672E12");

            entity.HasOne(d => d.IdAvailableNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdAvailable)
                .HasConstraintName("FK__Booking__ID_Avai__7C4F7684");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdService)
                .HasConstraintName("FK__Booking__ID_Serv__7B5B524B");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Class__3214EC2778159DE8");

            entity.ToTable("Class");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.ClassName).HasMaxLength(50);
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Complain__3214EC27BC0FAEE9");

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
                .HasConstraintName("FK__Complaint__ID_Ac__4F7CD00D");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Complaint__ID_Tu__5070F446");
        });

        modelBuilder.Entity<EducationalQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Educatio__3214EC2799016C20");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");
            entity.Property(e => e.QualificationName).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.EducationalQualifications)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Education__ID_Tu__5CD6CB2B");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC276A977FAC");

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
                .HasConstraintName("FK__RefreshTo__ID_Ac__59FA5E80");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rent__3214EC2757667FEA");

            entity.ToTable("Rent");

            entity.HasIndex(e => e.IdRequest, "UQ__Rent__D550988170471CCC").IsUnique();

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
                .HasConstraintName("FK__Rent__ID_Request__00200768");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request__3214EC27D2DA6CDA");

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
                .HasConstraintName("FK__Request__ID_Acco__6754599E");

            entity.HasOne(d => d.IdClassNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdClass)
                .HasConstraintName("FK__Request__ID_Clas__68487DD7");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdSubject)
                .HasConstraintName("FK__Request__ID_Subj__693CA210");
        });

        modelBuilder.Entity<RequestLearning>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request___3214EC27D5F42003");

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
                .HasConstraintName("FK__Request_L__ID_Re__6D0D32F4");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.RequestLearnings)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Request_L__ID_Tu__6C190EBB");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3214EC27734F9F0D");

            entity.ToTable("Review");

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

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Review__ID_Accou__534D60F1");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Review__ID_Tutor__5441852A");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC27E4D62BEA");

            entity.ToTable("Service");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdClass)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Class");
            entity.Property(e => e.IdSubject)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Subject");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.IdClassNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdClass)
                .HasConstraintName("FK__Service__ID_Clas__70DDC3D8");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdSubject)
                .HasConstraintName("FK__Service__ID_Subj__71D1E811");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Service__ID_Tuto__6FE99F9F");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3214EC27F77CB9CA");

            entity.ToTable("Subject");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.SubjectName).HasMaxLength(50);
        });

        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TimeSlot__3214EC2758F8C618");

            entity.ToTable("TimeSlot");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.TimeSlot1).HasColumnName("TimeSlot");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC27BDA8DB2E");

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
                .HasConstraintName("FK__Transacti__ID_Ac__571DF1D5");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor__3214EC271B4D801A");

            entity.ToTable("Tutor");

            entity.HasIndex(e => e.IdAccount, "UQ__Tutor__213379EAC617C8CA").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.SpecializedSkills).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.IdAccountNavigation).WithOne(p => p.Tutor)
                .HasForeignKey<Tutor>(d => d.IdAccount)
                .HasConstraintName("FK__Tutor__ID_Accoun__4CA06362");
        });

        modelBuilder.Entity<TutorSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor_Su__3214EC27B2D0A28B");

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
                .HasConstraintName("FK__Tutor_Sub__ID_Su__628FA481");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.TutorSubjects)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Tutor_Sub__ID_Tu__619B8048");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
