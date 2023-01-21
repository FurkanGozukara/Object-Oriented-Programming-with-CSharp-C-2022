﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace lecture_14_crawler_ex.Models;

public partial class ExampleCrawlerContext : DbContext
{
    public ExampleCrawlerContext()
    {
    }

    public ExampleCrawlerContext(DbContextOptions<ExampleCrawlerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RootDomains> RootDomains { get; set; }

    public virtual DbSet<Urls> Urls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=ExampleCrawler;Integrated Security=True;encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RootDomains>(entity =>
        {
            entity.HasKey(e => e.RootDomainId);

            entity.Property(e => e.RootDomainUrlHash)
                .IsRequired()
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Urls>(entity =>
        {
            entity.HasKey(e => e.UrlHash);

            entity.HasIndex(e => new { e.Crawled, e.DiscoveryDate }, "crawled_discoverydate");

            entity.Property(e => e.UrlHash)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DepthLevel).HasComment("0");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.DiscoveryDate)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastCrawlDate)
                .HasDefaultValueSql("(((1900)-(1))-(1))")
                .HasColumnType("datetime");
            entity.Property(e => e.LinkText).HasMaxLength(2000);
            entity.Property(e => e.ParentUrlHash)
                .IsRequired()
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Title).HasMaxLength(2000);
            entity.Property(e => e.Url)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}