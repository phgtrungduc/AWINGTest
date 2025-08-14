using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AWP.DBContext.Models;

public partial class AwingDbContext : DbContext
{
    public AwingDbContext()
    {
    }

    public AwingDbContext(DbContextOptions<AwingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PuzzleMap> PuzzleMaps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.24.63.11,1433;Database=AwingDB;User Id=kiotvietdev;Password=C1t1g000$6162;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PuzzleMap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PuzzleMa__3214EC0754867C3C");

            entity.ToTable("PuzzleMap");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
