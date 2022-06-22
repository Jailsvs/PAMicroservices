﻿// <auto-generated />
using System;
using AuctionMicroservice.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuctionMicroservice.Migrations
{
    [DbContext(typeof(AuctionContext))]
    [Migration("20191104214930_InitialCreateAuction")]
    partial class InitialCreateAuction
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AuctionMicroservice.Models.AuctionBid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuctionProductId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BidDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tb_AuctionBid");
                });

            modelBuilder.Entity("AuctionMicroservice.Models.AuctionProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("BidAmount")
                        .HasColumnType("float");

                    b.Property<double>("BidValue")
                        .HasColumnType("float");

                    b.Property<string>("Closed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<double>("CurrentValue")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastBidDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MinValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("OpeningDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StopwatchTime")
                        .HasColumnType("int");

                    b.Property<int>("TenantId")
                        .HasColumnType("int");

                    b.Property<string>("URLDescExt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URLImg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WinnerAuctionUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tb_AuctionProduct");
                });
#pragma warning restore 612, 618
        }
    }
}