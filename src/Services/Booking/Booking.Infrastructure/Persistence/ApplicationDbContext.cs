using System.Reflection;
using Booking.Domain.AggregationRoots.Booking;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public IQueryable<BookingEntity> NonDeletedBookings => Set<BookingEntity>().Where(x => !x.IsDeleted);
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(
            Assembly.GetAssembly(GetType()) ?? throw new InvalidOperationException());
    }
}