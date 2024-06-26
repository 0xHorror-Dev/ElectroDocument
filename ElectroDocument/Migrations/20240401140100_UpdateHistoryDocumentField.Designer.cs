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
    [DbContext(typeof(ElectroDocumentContext))]
    [Migration("20240401140100_UpdateHistoryDocumentField")]
    partial class UpdateHistoryDocumentField
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

                    b.Property<string>("DescFirst")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("descFirst");

                    b.Property<string>("DescSecond")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("descSecond");

                    b.Property<DateOnly?>("FirstDay")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("SecondDay")
                        .HasColumnType("date");

                    b.Property<sbyte?>("Type")
                        .HasColumnType("tinyint(4)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("Docs");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Employee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20)");

                    b.Property<long>("CredentialsId")
                        .HasColumnType("bigint(20)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("longtext");

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

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.History", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .HasDefaultValueSql("'0'");

                    b.Property<long?>("Document")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20)")
                        .HasDefaultValueSql("'0'");

                    b.Property<long>("Employee")
                        .HasColumnType("bigint(20)");

                    b.Property<long>("Owner")
                        .HasColumnType("bigint(20)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Document" }, "FK1_History_Document");

                    b.HasIndex(new[] { "Owner" }, "FK2_History_Owner");

                    b.HasIndex(new[] { "Employee" }, "FK3_History_Employee");

                    b.ToTable("History", (string)null);
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Individual", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint(20)");

                    b.Property<string>("Address")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValueSql("''");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValueSql("''");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
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

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.History", b =>
                {
                    b.HasOne("ElectroDocument.Controllers.AppContext.Doc", "DocumentNavigation")
                        .WithMany("Histories")
                        .HasForeignKey("Document")
                        .HasConstraintName("FK1_History_Document");

                    b.HasOne("ElectroDocument.Controllers.AppContext.Employee", "EmployeeNavigation")
                        .WithMany("HistoryEmployeeNavigations")
                        .HasForeignKey("Employee")
                        .IsRequired()
                        .HasConstraintName("FK3_History_Employee");

                    b.HasOne("ElectroDocument.Controllers.AppContext.Employee", "OwnerNavigation")
                        .WithMany("HistoryOwnerNavigations")
                        .HasForeignKey("Owner")
                        .IsRequired()
                        .HasConstraintName("FK2_History_Owner");

                    b.Navigation("DocumentNavigation");

                    b.Navigation("EmployeeNavigation");

                    b.Navigation("OwnerNavigation");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Doc", b =>
                {
                    b.Navigation("Histories");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Employee", b =>
                {
                    b.Navigation("HistoryEmployeeNavigations");

                    b.Navigation("HistoryOwnerNavigations");
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
