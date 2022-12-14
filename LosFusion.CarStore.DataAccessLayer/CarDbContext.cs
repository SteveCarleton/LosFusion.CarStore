using Microsoft.EntityFrameworkCore;
using LosFusion.CarStore.BusinessLogicLayer.Entities;

namespace LosFusion.CarStore.DataAccessLayer;

public class CarDbContext : DbContext
{
    public DbSet<CarEntity>? Cars { get; set; }

    public CarDbContext(DbContextOptions<CarDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
}
