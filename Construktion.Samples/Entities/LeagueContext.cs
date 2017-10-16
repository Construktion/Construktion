using Microsoft.EntityFrameworkCore;

namespace Construktion.Samples.Entities
{
    public class LeagueContext : DbContext
    {
        public LeagueContext(DbContextOptions<LeagueContext> options)
            : base(options)
        {
           
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Agent> Agents { get; set; }
    }
}