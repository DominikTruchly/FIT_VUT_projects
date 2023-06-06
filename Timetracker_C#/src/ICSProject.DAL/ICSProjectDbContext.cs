namespace ICSProject.DAL;
using Microsoft.EntityFrameworkCore;
    using ICSProject.DAL.Entities;
    using ICSProject.DAL.Seeds;

public class ICSProjectDbContext : DbContext
{
    private readonly bool _seedDemoData;

    public ICSProjectDbContext(DbContextOptions contextOptions, bool seedDemoData = false)
        : base(contextOptions) => this._seedDemoData = seedDemoData;

    public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.UsrActivities)
            .WithOne(i => i.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasMany(i => i.UsrProjects)
            .WithOne(i => i.User);

        modelBuilder.Entity<ProjectEntity>()
            .HasMany(i => i.ProjActivities)
            .WithOne(i => i.Project)
            .OnDelete(DeleteBehavior.SetNull);

        if (_seedDemoData)
        {
            UserSeeds.Seed(modelBuilder);
            ActivitySeeds.Seed(modelBuilder);
            ProjectSeeds.Seed(modelBuilder);
        }
    }
}

