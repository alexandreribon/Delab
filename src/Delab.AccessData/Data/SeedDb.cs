using Delab.AccessData.Context;
using Delab.Shared.Entities;

namespace Delab.AccessData.Data;

public class SeedDb
{
    private readonly DBContext _dbContext;

    public SeedDb(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync()
    {
        await _dbContext.Database.EnsureCreatedAsync();

        await CheckCountries();
    }

    private async Task CheckCountries()
    {
        if (!_dbContext.Countries.Any())
        {
            await _dbContext.Countries.AddAsync(new Country
            {
                Name = "Brasil",
                CodPhone = "+55",
                States =
                [
                    new State 
                    { 
                        Name = "São Paulo", 
                        Cities =
                        [
                            new City { Name = "São Paulo" },
                            new City { Name = "Osasco" },
                            new City { Name = "Guarulhos" }
                        ] 
                    },
                    new State 
                    { 
                        Name = "Rio de Janeiro",
                        Cities = new[] 
                        { 
                            new City { Name = "Rio de Janeiro"},
                            new City { Name = "Petrópolis"},
                            new City { Name = "Niterói"}
                        }
                    }
                ]
            });

            await _dbContext.Countries.AddAsync(new Country
            {
                Name = "Colombia",
                CodPhone = "+57",
                States =
                [
                    new State
                    {
                        Name = "Antioquia",
                        Cities = new[] 
                        { 
                            new City { Name = "Medellin" },
                            new City { Name = "Itaqui" },
                            new City { Name = "Bello" }
                        }
                    }
                ]
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}
