﻿using Microsoft.EntityFrameworkCore;
using HelpdeskApp.Models;

namespace HelpdeskApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
