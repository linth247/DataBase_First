using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;

namespace WebAPI.Models;

public partial class WebContext : DbContext
{
    public WebContext(DbContextOptions<WebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employee { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsFiles> NewsFiles { get; set; }

    public virtual DbSet<Organ> Organ { get; set; }

    public virtual DbSet<TodoList> TodoList { get; set; }
    //public virtual DbSet<TodoListDto> TodoListDto { get; set; }

    public virtual DbSet<UploadFile> UploadFile { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<TodoListDto>().HasNoKey();

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.EmployeeId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Account)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.Property(e => e.NewsId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.EndDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InsertDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.StartDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.UpdateDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<NewsFiles>(entity =>
        {
            entity.Property(e => e.NewsFilesId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Extension)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.Path).IsRequired();
        });

        modelBuilder.Entity<Organ>(entity =>
        {
            entity.Property(e => e.OrganId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Src)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TodoList>(entity =>
        {
            entity.HasKey(e => e.TodoId);

            entity.Property(e => e.TodoId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.InsertTime).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.UpdateTime).HasColumnType("datetime");

            entity.HasOne(d => d.InsertEmployee).WithMany(p => p.TodoListInsertEmployee)
                .HasForeignKey(d => d.InsertEmployeeId)
                .HasConstraintName("FK_TodoList_Employee");

            entity.HasOne(d => d.UpdateEmployee).WithMany(p => p.TodoListUpdateEmployee)
                .HasForeignKey(d => d.UpdateEmployeeId)
                .HasConstraintName("FK_TodoList_Employee1");
        });

        modelBuilder.Entity<UploadFile>(entity =>
        {
            entity.Property(e => e.UploadFileId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Src)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Todo).WithMany(p => p.UploadFiles)
                .HasForeignKey(d => d.TodoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UploadFile_TodoList");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
