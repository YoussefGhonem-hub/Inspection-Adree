using Inspection.Domain.Entities;
using Inspection.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Inspection.Infrastructure.Persistence;


public static class ConfigurationDbContextSeed
{
    public static async Task InitializeDatabase(AppDbContext context)
    {
        // Apply any pending migrations
        await context.Database.MigrateAsync();

        // Seed Inspectors if they do not exist
        await SeedInspectors(context);
    }

    private static async Task SeedInspectors(AppDbContext context)
    {
        // Check if there are already inspectors in the database
        if (!context.Inspectors.Any())
        {
            // Seed default inspectors if the table is empty
            var inspectors = new[]
            {
                new Inspector
                {
                    Id = Guid.NewGuid(),
                    FullName = "Admin Inspector",
                    Email = "admin@inspection.com",
                    Phone = "123-456-7890",
                    Role = Role.Admin  // Assuming Role is an enum with Admin role
                },
                new Inspector
                {
                    Id = Guid.NewGuid(),
                    FullName = "John Doe",
                    Email = "john.doe@inspection.com",
                    Phone = "987-654-3210",
                    Role = Role.Inspector  // Assuming Role is an enum with Inspector role
                },
                new Inspector
                {
                    Id = Guid.NewGuid(),
                    FullName = "Jane Smith",
                    Email = "jane.smith@inspection.com",
                    Phone = "555-123-4567",
                    Role = Role.Inspector  // Assuming Role is an enum with Inspector role
                }
            };

            // Add the default inspectors to the context
            await context.Inspectors.AddRangeAsync(inspectors);

            // Save changes to the database
            await context.SaveChangesAsync();
        }
    }
}