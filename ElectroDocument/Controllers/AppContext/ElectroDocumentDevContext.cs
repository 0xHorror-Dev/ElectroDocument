using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ElectroDocument.Controllers.AppContext;

public partial class ElectroDocumentDevContext : DbContext
{
    public ElectroDocumentDevContext()
    {
    }

    public ElectroDocumentDevContext(DbContextOptions<ElectroDocumentDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doc> Docs { get; set; }

    public virtual DbSet<DocDetail> DocDetails { get; set; }

    public virtual DbSet<DocumentVersion> DocumentVersions { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeCredential> EmployeeCredentials { get; set; }

    public virtual DbSet<Individual> Individuals { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=192.168.0.225;database=ElectroDocumentDev;uid=ElectroDocument;pwd=ElectroDocument", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.6-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Doc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.DocDetailsId, "FK_DocDetails_Docs");

            entity.HasIndex(e => e.EmployeeId, "FK_DocsNew_Employee");

            entity.HasIndex(e => e.Responsible, "FK_DocsNew_Responsible");

            entity.Property(e => e.Id).HasColumnType("bigint(20)");
            entity.Property(e => e.DocDetailsId).HasColumnType("bigint(20)");
            entity.Property(e => e.DocType).HasColumnType("tinyint(4)");
            entity.Property(e => e.EmployeeId).HasColumnType("bigint(20)");
            entity.Property(e => e.Notified).HasColumnType("smallint(6)");
            entity.Property(e => e.Number).HasColumnType("bigint(20)");
            entity.Property(e => e.Responsible).HasColumnType("bigint(20)");
            entity.Property(e => e.ResponsibleNotified)
                .HasDefaultValueSql("'0'")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Sum).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.DocDetails).WithMany(p => p.Docs)
                .HasForeignKey(d => d.DocDetailsId)
                .HasConstraintName("FK_DocDetails_Docs");

            entity.HasOne(d => d.Employee).WithMany(p => p.DocEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_DocsNew_Employee");

            entity.HasOne(d => d.ResponsibleNavigation).WithMany(p => p.DocResponsibleNavigations)
                .HasForeignKey(d => d.Responsible)
                .HasConstraintName("FK_DocsNew_Responsible");
        });

        modelBuilder.Entity<DocDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id).HasColumnType("bigint(20)");
            entity.Property(e => e.Desc).HasColumnType("text");
            entity.Property(e => e.DescSecond).HasColumnType("text");
            entity.Property(e => e.Reason).HasColumnType("text");
        });

        modelBuilder.Entity<DocumentVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DocumentVersion");

            entity.HasIndex(e => e.DocId, "FK1_DocId_Docs_Id");

            entity.HasIndex(e => e.NewDocId, "FK2_NewDocId_Docs_Id");

            entity.HasIndex(e => e.DocIdSrc, "FK3_DocIdSrc_Docs_Id");

            entity.HasIndex(e => e.EditorId, "FK4_EditorId_Employee_Id");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DocId).HasColumnType("bigint(20)");
            entity.Property(e => e.DocIdSrc).HasColumnType("bigint(20)");
            entity.Property(e => e.EditorId).HasColumnType("bigint(20)");
            entity.Property(e => e.NewDocId).HasColumnType("bigint(20)");

            entity.HasOne(d => d.Doc).WithMany(p => p.DocumentVersionDocs)
                .HasForeignKey(d => d.DocId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK1_DocId_Docs_Id");

            entity.HasOne(d => d.DocIdSrcNavigation).WithMany(p => p.DocumentVersionDocIdSrcNavigations)
                .HasForeignKey(d => d.DocIdSrc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK3_DocIdSrc_Docs_Id");

            entity.HasOne(d => d.Editor).WithMany(p => p.DocumentVersions)
                .HasForeignKey(d => d.EditorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK4_EditorId_Employee_Id");

            entity.HasOne(d => d.NewDoc).WithMany(p => p.DocumentVersionNewDocs)
                .HasForeignKey(d => d.NewDocId)
                .HasConstraintName("FK2_NewDocId_Docs_Id");
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
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValueSql("''");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(250)
                .HasDefaultValueSql("''");
            entity.Property(e => e.PhoneNumber).HasMaxLength(250);
            entity.Property(e => e.Surname)
                .HasMaxLength(250)
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
