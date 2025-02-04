using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MRBS.Core.Models;

public partial class MrbsContext : DbContext
{
    public MrbsContext()
    {
    }

    public MrbsContext(DbContextOptions<MrbsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Company__3213E83F838B9B47");

            entity.ToTable("Company");

            entity.HasIndex(e => e.EmailAddress, "UQ__Company__347C3027182FADC9").IsUnique();

            entity.HasIndex(e => e.Name, "UQ__Company__72E12F1B00F23964").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("emailAddress");
            entity.Property(e => e.Logo).HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservat__3213E83FB44C233D");

            entity.ToTable("Reservation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOfMeeting)
                .HasColumnType("date")
                .HasColumnName("dateOfMeeting");
            entity.Property(e => e.EndTime)
                .HasPrecision(0)
                .HasColumnName("endTime");
            entity.Property(e => e.MeetingStatus).HasColumnName("meetingStatus");
            entity.Property(e => e.NumberAttendees).HasColumnName("numberAttendees");
            entity.Property(e => e.RoomId).HasColumnName("roomId");
            entity.Property(e => e.ScreenNedded).HasColumnName("screenNedded");
            entity.Property(e => e.StartTime)
                .HasPrecision(0)
                .HasColumnName("startTime");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Room).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__roomI__34C8D9D1");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservati__userI__35BCFE0A");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3213E83F717F4A09");

            entity.ToTable("Room");

            entity.HasIndex(e => e.Name, "UQ__Room__72E12F1BA5CFF375").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.Location)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.RoomDescription)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("roomDescription");

            entity.HasOne(d => d.Company).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Room__companyId__31EC6D26");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F2AC3CD05");

            entity.ToTable("User");

            entity.HasIndex(e => e.PhoneNumber, "UQ__User__4849DA01B003D647").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D1053498BD1E1B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("date")
                .HasColumnName("dateOfBirth");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PasswordResetToken)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.ResetTokenExpires).HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.VerificationToken)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VerifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.Users)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__companyId__2E1BDC42");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}