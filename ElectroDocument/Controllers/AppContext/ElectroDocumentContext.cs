using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ElectroDocument.Controllers.AppContext;

public partial class ElectroDocumentContext : DbContext
{
    public ElectroDocumentContext()
    {
    }

    public ElectroDocumentContext(DbContextOptions<ElectroDocumentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doc> Docs { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeCredential> EmployeeCredentials { get; set; }

    public virtual DbSet<Individual> Individuals { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.0.225;database=ElectroDocument;uid=ElectroDocument;pwd=ElDocsas2", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.6-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Doc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.EmployeeId, "FK_DocsNew_Employee");

            entity.HasIndex(e => e.Number, "Number").IsUnique();

            entity.Property(e => e.Id).HasColumnType("bigint(20)");
            entity.Property(e => e.Desc).HasColumnType("text");
            entity.Property(e => e.DescSecond).HasColumnType("text");
            entity.Property(e => e.DocType).HasColumnType("tinyint(4)");
            entity.Property(e => e.EmployeeId).HasColumnType("bigint(20)");
            entity.Property(e => e.Number).HasColumnType("bigint(20)");
            entity.Property(e => e.Reason).HasColumnType("text");
            entity.Property(e => e.Sum).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.Docs)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_DocsNew_Employee");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.RoleId, "FK_Employee_Role");

            entity.HasIndex(e => e.IndividualId, "FK_Users_Individual_InvidividualId");

            entity.HasIndex(e => e.CredentialsId, "IX_Users_CredentialsId");

            entity.Property(e => e.Id).HasColumnType("bigint(20)");
            entity.Property(e => e.CredentialsId).HasColumnType("bigint(20)");
            entity.Property(e => e.IndividualId).HasColumnType("bigint(20)");
            entity.Property(e => e.RoleId).HasColumnType("bigint(20)");

            entity.HasOne(d => d.Credentials).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CredentialsId)
                .HasConstraintName("FK_Users_UserCredentials_CredentialsId");

            entity.HasOne(d => d.Individual).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IndividualId)
                .HasConstraintName("FK_Users_Individual_InvidividualId");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Employee_Role");
        });

        modelBuilder.Entity<EmployeeCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasColumnType("bigint(20)");
        });

        modelBuilder.Entity<Individual>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Individual");

            entity.Property(e => e.Id).HasColumnType("bigint(20)");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .HasDefaultValueSql("''");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnType("bigint(20)");
            entity.Property(e => e.AccessLevel)
                .HasDefaultValueSql("'User'")
                .HasColumnType("text");
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
