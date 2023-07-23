﻿// <auto-generated />
using System;
using DataTrack.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataTrack.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230723153238_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DataTrack.Model.Alarm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("AnalogInputId")
                        .HasColumnType("char(36)");

                    b.Property<double>("EdgeValue")
                        .HasColumnType("double");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AnalogInputId");

                    b.ToTable("Alarm");
                });

            modelBuilder.Entity("DataTrack.Model.AnalogInput", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Driver")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("HighLimit")
                        .HasColumnType("double");

                    b.Property<string>("IOAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("LowLimit")
                        .HasColumnType("double");

                    b.Property<bool>("ScanOn")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("ScanTime")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AnalogInput");
                });

            modelBuilder.Entity("DataTrack.Model.AnalogOutput", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("HighLimit")
                        .HasColumnType("double");

                    b.Property<string>("IOAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("LowLimit")
                        .HasColumnType("double");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AnalogOutput");
                });

            modelBuilder.Entity("DataTrack.Model.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("IOAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDigital")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("LowerBound")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UpperBound")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("DataTrack.Model.DigitalInput", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Driver")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("IOAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("ScanOn")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("ScanTime")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DigitalInput");
                });

            modelBuilder.Entity("DataTrack.Model.DigitalOutput", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("IOAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("InitialValue")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("DigitalOutput");
                });

            modelBuilder.Entity("DataTrack.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Admin")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("AnalogInputId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("DigitalInputId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("RegisteredById")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("AnalogInputId");

                    b.HasIndex("DigitalInputId");

                    b.HasIndex("RegisteredById");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DataTrack.Model.Alarm", b =>
                {
                    b.HasOne("DataTrack.Model.AnalogInput", "AnalogInput")
                        .WithMany("Alarms")
                        .HasForeignKey("AnalogInputId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnalogInput");
                });

            modelBuilder.Entity("DataTrack.Model.User", b =>
                {
                    b.HasOne("DataTrack.Model.AnalogInput", null)
                        .WithMany("Users")
                        .HasForeignKey("AnalogInputId");

                    b.HasOne("DataTrack.Model.DigitalInput", null)
                        .WithMany("Users")
                        .HasForeignKey("DigitalInputId");

                    b.HasOne("DataTrack.Model.User", "RegisteredBy")
                        .WithMany()
                        .HasForeignKey("RegisteredById");

                    b.Navigation("RegisteredBy");
                });

            modelBuilder.Entity("DataTrack.Model.AnalogInput", b =>
                {
                    b.Navigation("Alarms");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("DataTrack.Model.DigitalInput", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}