using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;

namespace WebAPI.Models;

public partial class WebContext2 : WebContext
{
    public WebContext2(DbContextOptions<WebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TodoListDto> TodoListDto { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TodoListDto>().HasNoKey();
 
    }

    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder); 

}
