using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Debit> Debits { get; set; }

    public DbSet<AppUser> Users { get; set; }
}