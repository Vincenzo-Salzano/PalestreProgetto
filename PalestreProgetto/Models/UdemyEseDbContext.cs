using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PalestreProgetto.Models;

public partial class UdemyEseDbContext : DbContext
{
    public UdemyEseDbContext()
    {
    }

    public UdemyEseDbContext(DbContextOptions<UdemyEseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attrezzo> Attrezzos { get; set; }

    public virtual DbSet<Palestra> Palestras { get; set; }

    public virtual DbSet<PalestraAttrezzo> PalestraAttrezzos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-L513NFK\\SQLEXPRESS;Database=UdemyEseDB;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attrezzo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attrezzo__3214EC077AE96603");

            entity.ToTable("Attrezzo");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Palestra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Palestra__3214EC0713AEA9D6");

            entity.ToTable("Palestra");

            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PalestraAttrezzo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Palestra__3214EC07639AE3AB");

            entity.ToTable("PalestraAttrezzo");

            entity.Property(e => e.AttrezzoId).HasColumnName("Attrezzo_id");
            entity.Property(e => e.PalestraId).HasColumnName("Palestra_id");

            entity.HasOne(d => d.Attrezzo).WithMany(p => p.PalestraAttrezzos)
                .HasForeignKey(d => d.AttrezzoId)
                .HasConstraintName("AttrezzoFK");

            entity.HasOne(d => d.Palestra).WithMany(p => p.PalestraAttrezzos)
                .HasForeignKey(d => d.PalestraId)
                .HasConstraintName("PalestraFK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
