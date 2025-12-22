using Microsoft.EntityFrameworkCore;
using Server.Common.Database;
using Server.Modules.Countries.Entities;

namespace Server.Modules.Countries;

public class CountriesService(AppDbContext dbContext)
{
    public async Task<List<CountryEntity>> GetCountries()
    {
        return await dbContext.Countries.AsNoTracking().ToListAsync();
    }

    public async Task<CountryEntity?> GetCountryByCode(string code)
    {
        return await dbContext.Countries.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code);
    }
}