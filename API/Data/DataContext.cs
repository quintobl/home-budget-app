using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Debit> Debits { get; set; }

    public DbSet<AppUser> Users { get; set; }

    public DbSet<Account> Accounts { get; set; }

    public DbSet<Credit> Credits { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Description> Descriptions { get; set; }
}