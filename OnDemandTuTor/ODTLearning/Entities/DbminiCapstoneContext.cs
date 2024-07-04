﻿using System;
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

    public virtual DbSet<ClassRequest> ClassRequests { get; set; }

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
        => optionsBuilder.UseSqlServer("Server=(local);Database=DBMiniCapstone;UID=sa;PWD=12345;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC27047EFD44");

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
            entity.HasKey(e => e.Id).HasName("PK__Availabl__3214EC27644D3531");

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
                .HasConstraintName("FK__Available__ID_Ti__6477ECF3");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Availables)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Available__ID_Tu__6383C8BA");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Booking__3214EC27AD01E5FD");

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
                .HasConstraintName("FK__Booking__ID_Acco__6754599E");

            entity.HasOne(d => d.IdAvailableNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdAvailable)
                .HasConstraintName("FK__Booking__ID_Avai__693CA210");

            entity.HasOne(d => d.IdServiceNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdService)
                .HasConstraintName("FK__Booking__ID_Serv__68487DD7");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Class__3214EC27748BA6CE");

            entity.ToTable("Class");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.ClassName).HasMaxLength(50);
        });

        modelBuilder.Entity<ClassRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClassReq__3214EC27DC8CDF8C");

            entity.ToTable("ClassRequest");

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

            entity.HasOne(d => d.IdRequestNavigation).WithMany(p => p.ClassRequests)
                .HasForeignKey(d => d.IdRequest)
                .HasConstraintName("FK__ClassRequ__ID_Re__6FE99F9F");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.ClassRequests)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__ClassRequ__ID_Tu__70DDC3D8");
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Complain__3214EC270346BF42");

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
                .HasConstraintName("FK__Complaint__ID_Ac__3C69FB99");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Complaint__ID_Tu__3D5E1FD2");
        });

        modelBuilder.Entity<EducationalQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Educatio__3214EC27A629387A");

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
                .HasConstraintName("FK__Education__ID_Tu__49C3F6B7");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC275A39A699");

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
                .HasConstraintName("FK__RefreshTo__ID_Ac__46E78A0C");
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rent__3214EC27C39AD5D1");

            entity.ToTable("Rent");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IdAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Account");
            entity.Property(e => e.IdTutor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Tutor");

            entity.HasOne(d => d.IdAccountNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdAccount)
                .HasConstraintName("FK__Rent__ID_Account__6C190EBB");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Rents)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Rent__ID_Tutor__6D0D32F4");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request__3214EC2791C3F130");

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
                .HasConstraintName("FK__Request__ID_Acco__5441852A");

            entity.HasOne(d => d.IdClassNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdClass)
                .HasConstraintName("FK__Request__ID_Clas__5535A963");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdSubject)
                .HasConstraintName("FK__Request__ID_Subj__5629CD9C");
        });

        modelBuilder.Entity<RequestLearning>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Request___3214EC274A03DA81");

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
                .HasConstraintName("FK__Request_L__ID_Re__59FA5E80");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.RequestLearnings)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Request_L__ID_Tu__59063A47");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3214EC27B5524C88");

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
                .HasConstraintName("FK__Review__ID_Accou__403A8C7D");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Review__ID_Tutor__412EB0B6");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC273C2359DD");

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
                .HasConstraintName("FK__Service__ID_Clas__5DCAEF64");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdSubject)
                .HasConstraintName("FK__Service__ID_Subj__5EBF139D");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.Services)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Service__ID_Tuto__5CD6CB2B");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3214EC2795603281");

            entity.ToTable("Subject");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.SubjectName).HasMaxLength(50);
        });

        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TimeSlot__3214EC273AC713D2");

            entity.ToTable("TimeSlot");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.TimeSlot1).HasColumnName("TimeSlot");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC27AC970B5B");

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
                .HasConstraintName("FK__Transacti__ID_Ac__440B1D61");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor__3214EC271BB0FAA1");

            entity.ToTable("Tutor");

            entity.HasIndex(e => e.IdAccount, "UQ__Tutor__213379EA9366E556").IsUnique();

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
                .HasConstraintName("FK__Tutor__ID_Accoun__398D8EEE");
        });

        modelBuilder.Entity<TutorSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tutor_Su__3214EC2730C29ECF");

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
                .HasConstraintName("FK__Tutor_Sub__ID_Su__4F7CD00D");

            entity.HasOne(d => d.IdTutorNavigation).WithMany(p => p.TutorSubjects)
                .HasForeignKey(d => d.IdTutor)
                .HasConstraintName("FK__Tutor_Sub__ID_Tu__4E88ABD4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
