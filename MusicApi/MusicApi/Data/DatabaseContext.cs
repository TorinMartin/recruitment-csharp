using Microsoft.EntityFrameworkCore;
using MusicApi.Model;

namespace MusicApi.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Album> Albums { get; set; } = null!;
    public DbSet<Song> Songs { get; set; } = null!;

    private IConfiguration _configuration;
    
    public DatabaseContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_configuration.GetValue<string>("DbConnection"));
    }
}