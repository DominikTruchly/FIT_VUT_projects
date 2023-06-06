using ICSProject.App.Options;
using ICSProject.DAL.Factories;
using ICSProject.DAL;
using ICSProject.DAL.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ICSProject.App;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(this IServiceCollection services, IConfiguration configuration)
    {
        DALOptions dalOptions = new();
        configuration.GetSection("ICSProject:DAL").Bind(dalOptions);

        IConfigurationSection dalSection = configuration.GetSection("ICSProject:DAL");
        if (dalSection.Exists())
        {
            dalSection.Bind(dalOptions);
        }
        else
        {
            throw new InvalidOperationException("ICSProject:DAL section not found in the configuration file.");
        }

        services.AddSingleton<DALOptions>(dalOptions);

        if (dalOptions.Sqlite is null)
        {
            throw new InvalidOperationException("No persistence provider configured");
        }

        if (dalOptions.Sqlite?.Enabled == false)
        {
            throw new InvalidOperationException("No persistence provider enabled");
        }
        
        if (dalOptions.Sqlite?.Enabled == true)
        {
            if (dalOptions.Sqlite.DatabaseName is null)
            {
                throw new InvalidOperationException($"{nameof(dalOptions.Sqlite.DatabaseName)} is not set");

            }
            string databaseFilePath = Path.Combine(FileSystem.AppDataDirectory, dalOptions.Sqlite.DatabaseName!);
            services.AddSingleton<IDbContextFactory<ICSProjectDbContext>>(provider => new DbContextSqLiteFactory(databaseFilePath, dalOptions?.Sqlite?.SeedDemoData ?? false));
            services.AddSingleton<IDbMigrator, SqliteDbMigrator>();
        }

        services.AddSingleton<ActivityEntityMapper>();
        services.AddSingleton<UserEntityMapper>();
        services.AddSingleton<ProjectEntityMapper>();

        return services;
    }
}
