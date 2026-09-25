using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;                                               
using Microsoft.EntityFrameworkCore;                                                                   
using MyTravel.Domain.Entities;  


namespace MyTravel.Infrastructure.Persistence; 

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid> 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)    
    {   
    }


    //DBset de dominio.
    public DbSet<Trip> Trips => Set<Trip>();
    public DbSet<ItineraryDay> ItineraryDays => Set<ItineraryDay>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<TripMember> TripMembers => Set<TripMember>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Claves compuestas
        builder.Entity<TripMember>()
                       .HasKey(m => new { m.TripId, m.UserId });

        builder.Entity<TripMember>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        //soft Delete y filtro automatico.
        builder.Entity<Trip>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<ItineraryDay>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Activity>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Expense>().HasQueryFilter(e => !e.IsDeleted);

        builder.Entity<Trip>()
                .Property(t => t.TotalBudget)
                .HasPrecision(18, 2);

        builder.Entity<Expense>()
            .Property(e => e.OriginalAmount)
            .HasPrecision(18, 2);

        builder.Entity<Expense>()
            .Property(e => e.ConvertedAmount)
            .HasPrecision(18, 2);

        builder.Entity<Expense>()
            .Property(e => e.ExchangeRateUsed)
            .HasPrecision(18, 6);

        builder.Entity<ItineraryDay>()
            .Property(d => d.TemperatureC)
            .HasPrecision(5, 2);
    }



}

