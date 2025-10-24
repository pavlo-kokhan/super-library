using Library.Domain.AggregationRoots.Library;
using Library.Domain.AggregationRoots.Room;
using Library.Domain.AggregationRoots.ValueObjects;
using Library.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure;

public class DbSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public DbSeeder(ApplicationDbContext dbContext) => _dbContext = dbContext;
    
    public async Task SeedAsync()
    {
        if (!await _dbContext.Set<LibraryEntity>().AnyAsync() && !await _dbContext.Set<RoomEntity>().AnyAsync())
        {
            var rooms = new List<RoomEntity>
            {
                RoomEntity.Create(1, 1, true).Data,
                RoomEntity.Create(1, 2, true).Data,
                RoomEntity.Create(1, 2, true).Data,
                RoomEntity.Create(2, 1, true).Data,
                RoomEntity.Create(2, 2, true).Data
            };

            var libraries = new List<LibraryEntity>
            {
                LibraryEntity.Create("Manga Library",
                    new AddressValueObject("Ukraine", "Lviv", "Horodotska", "114a"),
                    WeekScheduleValueObject.Defaults,
                    1).Data,
                LibraryEntity.Create("Super Library",
                    new AddressValueObject("Ukraine", "Lviv", "Chornovola", "13"),
                    WeekScheduleValueObject.Defaults,
                    1).Data,
            };

            foreach (var room in rooms)
            {
                await _dbContext.AddAsync(room);
            }
        
            foreach (var library in libraries)
            {
                await _dbContext.AddAsync(library);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}