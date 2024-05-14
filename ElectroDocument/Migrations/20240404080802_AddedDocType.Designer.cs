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
    [Migration("20240404080802_AddedDocType")]
    partial class AddedDocType
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

                    b.Property<string>("Desc")
                        .HasColumnType("text");

                    b.Property<sbyte?>("DocType")
                        .HasColumnType("tinyint(4)");

                    b.Property<long?>("EmployeeId")
                        .HasColumnType("bigint(20)");

                    b.Property<long>("Number")
                        .HasColumnType("bigint(20)");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<int?>("Sum")
                        .HasColumnType("int(11)");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "EmployeeId" }, "FK_DocsNew_Employee");

                    b.HasIndex(new[] { "Number" }, "Number")
                        .IsUnique();

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

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Doc", b =>
                {
                    b.HasOne("ElectroDocument.Controllers.AppContext.Employee", "Employee")
                        .WithMany("Docs")
                        .HasForeignKey("EmployeeId")
                        .HasConstraintName("FK_DocsNew_Employee");

                    b.Navigation("Employee");
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

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Employee", b =>
                {
                    b.Navigation("Docs");
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
