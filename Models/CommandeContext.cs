using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Restaurant.Models
{
    public class CommandeContext : DbContext
    {
        public CommandeContext(DbContextOptions<CommandeContext> options)
            : base(options)
        {
        }

        public DbSet<Commande> Commandes { get; set; } = null!;
        public DbSet<Plat> Plats { get; set; } = null!;
        public DbSet<Boisson> Boissons { get; set; } = null!;

    }

}