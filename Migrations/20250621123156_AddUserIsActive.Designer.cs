﻿// <auto-generated />
using System;
using DMCPortal.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DMCPortal.API.Migrations
{
    [DbContext(typeof(DMCPortalDBContext))]
    [Migration("20250621123156_AddUserIsActive")]
    partial class AddUserIsActive
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DMCPortal.API.Entities.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("userId"));

                    b.Property<bool>("OperationIsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("UserIsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("createdOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("emailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("lastLoggedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("middleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("mobileNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("userId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DMCPortal.API.Entities.UserSession", b =>
                {
                    b.Property<Guid>("sessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("createdOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("expiredOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isExpired")
                        .HasColumnType("bit");

                    b.Property<string>("sessionHostName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sessionIPAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("sessionId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("Operation", b =>
                {
                    b.Property<int>("OperationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OperationId"));

                    b.Property<DateTime>("OperationCreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("OperationDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OperationIsActive")
                        .HasColumnType("bit");

                    b.Property<string>("OperationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OperationId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<DateTime>("RoleCreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("RoleDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("RoleOperation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("OperationId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OperationId");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleOperations");
                });

            modelBuilder.Entity("TruvaiQuery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AgentID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Budget")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ConfirmationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ConversionProbability")
                        .HasColumnType("int");

                    b.Property<string>("CostSheetLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EndClient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FinalPax")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GitFit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HandledBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastReplied")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OriginatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaxCount")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QueryCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("QueryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("QuoteUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ReminderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReminderOverdue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReservationLead")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TravelPlans")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WhyLost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Zone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TruvaiQueries");
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("RoleOperation", b =>
                {
                    b.HasOne("Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Operation");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.HasOne("Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DMCPortal.API.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DMCPortal.API.Entities.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
