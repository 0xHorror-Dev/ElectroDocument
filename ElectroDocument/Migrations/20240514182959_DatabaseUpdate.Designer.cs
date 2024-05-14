﻿// <auto-generated />
using System;
using ElectroDocument.Controllers.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ElectroDocument.Migrations
{
    [DbContext(typeof(ElectroDocumentDevContext))]
    [Migration("20240514182959_DatabaseUpdate")]
    partial class DatabaseUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Doc", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DateSecond")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DateThird")
                        .HasColumnType("date");

                    b.Property<long?>("DocDetailsId")
                        .HasColumnType("bigint(20)");

                    b.Property<sbyte?>("DocType")
                        .HasColumnType("tinyint(4)");

                    b.Property<long?>("EmployeeId")
                        .HasColumnType("bigint(20)");

                    b.Property<short>("Notified")
                        .HasColumnType("smallint(6)");

                    b.Property<long>("Number")
                        .HasColumnType("bigint(20)");

                    b.Property<long?>("Responsible")
                        .HasColumnType("bigint(20)");

                    b.Property<short?>("ResponsibleNotified")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint(6)")
                        .HasDefaultValueSql("'0'");

                    b.Property<int?>("Sum")
                        .HasColumnType("int(11)");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "DocDetailsId" }, "FK_DocDetails_Docs");

                    b.HasIndex(new[] { "EmployeeId" }, "FK_DocsNew_Employee");

                    b.HasIndex(new[] { "Responsible" }, "FK_DocsNew_Responsible");

                    b.ToTable("Docs");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.DocDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20)");

                    b.Property<string>("Desc")
                        .HasColumnType("text");

                    b.Property<string>("DescSecond")
                        .HasColumnType("text");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("DocDetails");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.DocumentVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<long>("DocId")
                        .HasColumnType("bigint(20)");

                    b.Property<long>("DocIdSrc")
                        .HasColumnType("bigint(20)");

                    b.Property<long>("EditorId")
                        .HasColumnType("bigint(20)");

                    b.Property<long?>("NewDocId")
                        .HasColumnType("bigint(20)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "DocId" }, "FK1_DocId_Docs_Id");

                    b.HasIndex(new[] { "NewDocId" }, "FK2_NewDocId_Docs_Id");

                    b.HasIndex(new[] { "DocIdSrc" }, "FK3_DocIdSrc_Docs_Id");

                    b.HasIndex(new[] { "EditorId" }, "FK4_EditorId_Employee_Id");

                    b.ToTable("DocumentVersion", (string)null);
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Employee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20)");

                    b.Property<long>("CredentialsId")
                        .HasColumnType("bigint(20)");

                    b.Property<long>("IndividualId")
                        .HasColumnType("bigint(20)");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint(20)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "RoleId" }, "FK_Employee_Role");

                    b.HasIndex(new[] { "IndividualId" }, "FK_Users_Individual_InvidividualId");

                    b.HasIndex(new[] { "CredentialsId" }, "IX_Users_CredentialsId");

                    b.ToTable("Employee", (string)null);
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.EmployeeCredential", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("EmployeeCredentials");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Individual", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20)");

                    b.Property<string>("Address")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValueSql("''");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasDefaultValueSql("''");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasDefaultValueSql("''");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("Individual", (string)null);
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20)");

                    b.Property<string>("AccessLevel")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValueSql("'User'");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Doc", b =>
                {
                    b.HasOne("ElectroDocument.Controllers.AppContext.DocDetail", "DocDetails")
                        .WithMany("Docs")
                        .HasForeignKey("DocDetailsId")
                        .HasConstraintName("FK_DocDetails_Docs");

                    b.HasOne("ElectroDocument.Controllers.AppContext.Employee", "Employee")
                        .WithMany("DocEmployees")
                        .HasForeignKey("EmployeeId")
                        .HasConstraintName("FK_DocsNew_Employee");

                    b.HasOne("ElectroDocument.Controllers.AppContext.Employee", "ResponsibleNavigation")
                        .WithMany("DocResponsibleNavigations")
                        .HasForeignKey("Responsible")
                        .HasConstraintName("FK_DocsNew_Responsible");

                    b.Navigation("DocDetails");

                    b.Navigation("Employee");

                    b.Navigation("ResponsibleNavigation");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.DocumentVersion", b =>
                {
                    b.HasOne("ElectroDocument.Controllers.AppContext.Doc", "Doc")
                        .WithMany("DocumentVersionDocs")
                        .HasForeignKey("DocId")
                        .IsRequired()
                        .HasConstraintName("FK1_DocId_Docs_Id");

                    b.HasOne("ElectroDocument.Controllers.AppContext.Doc", "DocIdSrcNavigation")
                        .WithMany("DocumentVersionDocIdSrcNavigations")
                        .HasForeignKey("DocIdSrc")
                        .IsRequired()
                        .HasConstraintName("FK3_DocIdSrc_Docs_Id");

                    b.HasOne("ElectroDocument.Controllers.AppContext.Employee", "Editor")
                        .WithMany("DocumentVersions")
                        .HasForeignKey("EditorId")
                        .IsRequired()
                        .HasConstraintName("FK4_EditorId_Employee_Id");

                    b.HasOne("ElectroDocument.Controllers.AppContext.Doc", "NewDoc")
                        .WithMany("DocumentVersionNewDocs")
                        .HasForeignKey("NewDocId")
                        .HasConstraintName("FK2_NewDocId_Docs_Id");

                    b.Navigation("Doc");

                    b.Navigation("DocIdSrcNavigation");

                    b.Navigation("Editor");

                    b.Navigation("NewDoc");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Employee", b =>
                {
                    b.HasOne("ElectroDocument.Controllers.AppContext.EmployeeCredential", "Credentials")
                        .WithMany("Employees")
                        .HasForeignKey("CredentialsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Users_UserCredentials_CredentialsId");

                    b.HasOne("ElectroDocument.Controllers.AppContext.Individual", "Individual")
                        .WithMany("Employees")
                        .HasForeignKey("IndividualId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Users_Individual_InvidividualId");

                    b.HasOne("ElectroDocument.Controllers.AppContext.Role", "Role")
                        .WithMany("Employees")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Employee_Role");

                    b.Navigation("Credentials");

                    b.Navigation("Individual");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Doc", b =>
                {
                    b.Navigation("DocumentVersionDocIdSrcNavigations");

                    b.Navigation("DocumentVersionDocs");

                    b.Navigation("DocumentVersionNewDocs");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.DocDetail", b =>
                {
                    b.Navigation("Docs");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Employee", b =>
                {
                    b.Navigation("DocEmployees");

                    b.Navigation("DocResponsibleNavigations");

                    b.Navigation("DocumentVersions");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.EmployeeCredential", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Individual", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Role", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
