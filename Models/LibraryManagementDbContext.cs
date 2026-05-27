using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models;

public partial class LibraryManagementDbContext : DbContext
{
    public LibraryManagementDbContext()
    {
    }

    public LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Carousel> Carousels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Authors__70DAFC34B668419C");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Nationality).HasMaxLength(100);
            entity.Property(e => e.Website).HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Books__3DE0C2075F4B9470");

            entity.Property(e => e.BookCode).HasMaxLength(200);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PublishedYear).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Authors");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Categories");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BAD82C032");

            entity.HasIndex(e => e.Name, "UQ__Categori__737584F63D3457F9").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.LoanDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.Loans)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Loans_Books1");

            entity.HasOne(d => d.User).WithMany(p => p.Loans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Loans_Users1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C26A26219");

            entity.HasIndex(e => e.UserCode, "UQ__Users__1DF52D0CBF72221A").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534436C4BF7").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Fullname).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Status).HasDefaultValue(1);
            entity.Property(e => e.UserCode).HasMaxLength(200);
        });

        modelBuilder.Entity<Carousel>(entity =>
        {
            entity.ToTable("Carousel");

            entity.HasKey(e => e.CarouselId);

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
