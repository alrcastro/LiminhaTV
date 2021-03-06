﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using LiminhaTV.Data;

namespace LiminhaTV.Migrations
{
    [DbContext(typeof(ChannelContext))]
    [Migration("20170625231930_container")]
    partial class container
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("LiminhaTV.Models.Channel", b =>
                {
                    b.Property<int>("ChannelId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Category");

                    b.Property<string>("Group");

                    b.Property<string>("LogoURL");

                    b.Property<string>("Path");

                    b.Property<string>("Title");

                    b.HasKey("ChannelId");

                    b.ToTable("Channel");
                });

            modelBuilder.Entity("LiminhaTV.Models.ChProgram", b =>
                {
                    b.Property<int>("ChProgramId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Channel");

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("Title");

                    b.HasKey("ChProgramId");

                    b.ToTable("Program");
                });
        }
    }
}
