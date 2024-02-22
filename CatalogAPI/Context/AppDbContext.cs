using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options ) : base( options )
    {}

    public DbSet<Category>? Categorys { get; set; }
    public DbSet<Product>? Products { get; set; }
}
