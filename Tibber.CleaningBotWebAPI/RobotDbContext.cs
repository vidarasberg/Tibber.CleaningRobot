namespace Tibber.CleaningBotWebAPI;

using Microsoft.EntityFrameworkCore;

public class RobotDbContext : DbContext
{
    public DbSet<ExecutionRecord> Executions { get; set; }

    public RobotDbContext(DbContextOptions<RobotDbContext> options)
        : base(options)
    {
    }
}
