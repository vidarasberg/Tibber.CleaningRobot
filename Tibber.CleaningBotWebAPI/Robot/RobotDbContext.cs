using Microsoft.EntityFrameworkCore;

namespace Tibber.CleaningBotWebAPI.Robot;

public class RobotDbContext : DbContext
{
    public DbSet<ExecutionRecord> Executions { get; set; }

    public RobotDbContext(DbContextOptions<RobotDbContext> options)
        : base(options)
    {
    }
}
