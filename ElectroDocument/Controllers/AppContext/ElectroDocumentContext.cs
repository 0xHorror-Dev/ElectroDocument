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

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeCredential> EmployeeCredentials { get; set; }

    public virtual DbSet<Individual> Individuals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.0.225;database=ElectroDocument;uid=ElectroDocument;pwd=ElDocsas2", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.6-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.IndividualId, "FK_Users_Individual_InvidividualId");

            entity.HasIndex(e => e.CredentialsId, "IX_Users_CredentialsId");

            entity.Property(e => e.Id).HasColumnType("bigint(20)");
            entity.Property(e => e.CredentialsId).HasColumnType("bigint(20)");
            entity.Property(e => e.IndividualId).HasColumnType("bigint(20)");

            entity.HasOne(d => d.Credentials).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CredentialsId)
                .HasConstraintName("FK_Users_UserCredentials_CredentialsId");

            entity.HasOne(d => d.Individual).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IndividualId)
                .HasConstraintName("FK_Users_Individual_InvidividualId");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
