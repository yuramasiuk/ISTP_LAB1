using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PostServiceWebApplication.Models;

public partial class PostServiceContext : DbContext
{
    public PostServiceContext()
    {
    }

    public PostServiceContext(DbContextOptions<PostServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<LocationType> LocationTypes { get; set; }

    public virtual DbSet<Parcel> Parcels { get; set; }

    public virtual DbSet<ParcelHistory> ParcelHistories { get; set; }

    public virtual DbSet<ParcelType> ParcelTypes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-97Q88N1;Database=PostService; Trusted_Connection=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Clients");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PostOffices");

            entity.Property(e => e.Address).HasMaxLength(50);

            entity.HasOne(d => d.Type).WithMany(p => p.Locations)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Locations_LocationTypes");
        });

        modelBuilder.Entity<LocationType>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Parcel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Parcel");

            entity.HasOne(d => d.ClientFrom).WithMany(p => p.ParcelClientFroms)
                .HasForeignKey(d => d.ClientFromId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcels_ClientsFrom");

            entity.HasOne(d => d.ClientTo).WithMany(p => p.ParcelClientTos)
                .HasForeignKey(d => d.ClientToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcels_ClientsTo");

            entity.HasOne(d => d.Status).WithMany(p => p.Parcels)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcels_Statuses");

            entity.HasOne(d => d.Type).WithMany(p => p.Parcels)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcels_ParcelTypes");
        });

        modelBuilder.Entity<ParcelHistory>(entity =>
        {
            entity.ToTable("ParcelHistory");

            entity.Property(e => e.ArrivalDate).HasColumnType("datetime");

            entity.HasOne(d => d.Location).WithMany(p => p.ParcelHistories)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParcelHistory_Locations");

            entity.HasOne(d => d.Parcel).WithMany(p => p.ParcelHistories)
                .HasForeignKey(d => d.ParcelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParcelHistory_Parcels");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
