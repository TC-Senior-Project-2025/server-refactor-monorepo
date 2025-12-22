using Microsoft.EntityFrameworkCore;
using Server.Modules.Auth.Entities;
using Server.Modules.Countries.Entities;
using Server.Modules.SaveGames.Entities;
using Server.Modules.Users.Entities;

namespace Server.Common.Database;

/// <summary>
/// Database context for the application.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the Users table.
    /// </summary>
    public DbSet<UserEntity> Users { get; set; } = null!;
    /// <summary>
    /// Gets or sets the UserTokens table.
    /// </summary>
    public DbSet<UserTokenEntity> UserTokens { get; set; } = null!;

    /// <summary>
    /// Gets or sets the SaveGames table.
    /// </summary>
    public DbSet<SaveGameEntity> SaveGames { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the Countries table.
    /// </summary>
    public DbSet<CountryEntity> Countries { get; set; } = null!;
}
