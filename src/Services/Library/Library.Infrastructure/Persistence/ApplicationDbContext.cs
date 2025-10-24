using System.Reflection;
using Library.Domain.AggregationRoots.Library;
using Library.Domain.AggregationRoots.Room;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public IQueryable<LibraryEntity> NonDeletedLibraries => Set<LibraryEntity>().Where(x => !x.IsDeleted);
    
    public IQueryable<RoomEntity> NonDeletedRooms => Set<RoomEntity>().Where(x => !x.IsDeleted);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(
            Assembly.GetAssembly(GetType()) ?? throw new InvalidOperationException());
    }
}