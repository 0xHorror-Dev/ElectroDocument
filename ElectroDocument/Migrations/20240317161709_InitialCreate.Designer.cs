﻿// <auto-generated />
using ElectroDocument.Controllers.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ElectroDocument.Migrations
{
    [DbContext(typeof(ElectroDocumentDevContext))]
    [Migration("20240317161709_InitialCreate")]
    partial class InitialCreate
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

                    b.HasKey("Id")
                        .HasName("PRIMARY");

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

                    b.Navigation("Credentials");

                    b.Navigation("Individual");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.EmployeeCredential", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("ElectroDocument.Controllers.AppContext.Individual", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
