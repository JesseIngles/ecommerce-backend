using Kimbito.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kimbito.Infra.Database;

public class KimbitoDbContext : DbContext
{
    public KimbitoDbContext(DbContextOptions<KimbitoDbContext> options)
        : base(options)
    {
    }
    public DbSet<Utilizador> Utilizadores { get; set; }

    


}