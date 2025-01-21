using System;
using health_pal_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace health_pal_backend.Config;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<UserModel> Users { get ; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
