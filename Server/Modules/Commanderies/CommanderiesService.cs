using Microsoft.EntityFrameworkCore;
using Server.Common.Database;
using Server.Modules.Commanderies.Entities;

namespace Server.Modules.Commanderies;

public class CommanderiesService(AppDbContext dbContext)
{
    public async Task<List<CommanderyEntity>> GetCommanderies()
    {
        return await dbContext.Commanderies.AsNoTracking().ToListAsync();
    }

    public async Task<CommanderyEntity?> GetCommanderyByCode(string code)
    {
        return await dbContext.Commanderies.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code);
    }
}