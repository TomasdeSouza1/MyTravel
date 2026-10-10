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
    public DbSet<Flight> Flights => Set<Flight>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Claves compuestas y relaciones de TripMember
        builder.Entity<TripMember>()
            .HasKey(m => new { m.TripId, m.UserId });

        builder.Entity<TripMember>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Soft Delete y filtro automático global
        builder.Entity<Trip>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<ItineraryDay>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Activity>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Expense>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Flight>().HasQueryFilter(e => !e.IsDeleted);

        // User 
        builder.Entity<User>(b =>
        {
            b.Property(u => u.FullName).HasMaxLength(100).IsRequired();
        });

        // Trip
        builder.Entity<Trip>(b =>
        {
            b.Property(t => t.Title).HasMaxLength(120).IsRequired();
            b.Property(t => t.Description).HasMaxLength(1000);
            b.Property(t => t.DestinationCountry).HasMaxLength(100).IsRequired();
            b.Property(t => t.DestinationCity).HasMaxLength(100).IsRequired();
            b.Property(t => t.CoverImageUrl).HasMaxLength(500);
            b.Property(t => t.BaseCurrency).HasMaxLength(3).IsRequired();
            b.Property(t => t.TotalBudget).HasPrecision(18, 2);
            b.Property(t => t.InviteToken).HasMaxLength(64).IsRequired();

            b.HasIndex(t => t.InviteToken).IsUnique();
            b.HasIndex(t => t.UserId);

            b.HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            b.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Trips_TotalBudget", "\"TotalBudget\" >= 0");
                t.HasCheckConstraint("CK_Trips_Dates", "\"EndDate\" >= \"StartDate\"");
            });
        });

        // ItineraryDay
        builder.Entity<ItineraryDay>(b =>
        {
            b.Property(d => d.LocationCountry).HasMaxLength(100).IsRequired();
            b.Property(d => d.LocationCity).HasMaxLength(100).IsRequired();
            b.Property(d => d.WeatherSumm).HasMaxLength(200);
            b.Property(d => d.TemperatureC).HasPrecision(5, 2);
            b.Property(d => d.Notes).HasMaxLength(2000);

            b.HasIndex(d => new { d.TripId, d.DayNumber });

            b.ToTable(t =>
            {
                t.HasCheckConstraint("CK_ItineraryDays_DayNumber", "\"DayNumber\" > 0");
                t.HasCheckConstraint("CK_ItineraryDays_TemperatureC", "\"TemperatureC\" IS NULL OR (\"TemperatureC\" >= -60 AND \"TemperatureC\" <= 60)");
            });
        });

        // Activity
        builder.Entity<Activity>(b =>
        {
            b.Property(a => a.Name).HasMaxLength(150).IsRequired();
            b.Property(a => a.Address).HasMaxLength(300);
            b.Property(a => a.BookingReference).HasMaxLength(50);
            b.Property(a => a.Notes).HasMaxLength(1000);

            b.HasIndex(a => new { a.ItineraryDayId, a.OrderIndex });

            b.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Activities_Latitude", "\"Latitude\" >= -90.0 AND \"Latitude\" <= 90.0");
                t.HasCheckConstraint("CK_Activities_Longitude", "\"Longitude\" >= -180.0 AND \"Longitude\" <= 180.0");
                t.HasCheckConstraint("CK_Activities_OrderIndex", "\"OrderIndex\" >= 0");
                t.HasCheckConstraint("CK_Activities_Times", "\"EndTime\" >= \"StartTime\"");
            });
        });

        // Expense
        builder.Entity<Expense>(b =>
        {
            b.Property(e => e.OriginalCurrency).HasMaxLength(3).IsRequired();
            b.Property(e => e.OriginalAmount).HasPrecision(18, 2);
            b.Property(e => e.ConvertedAmount).HasPrecision(18, 2);
            b.Property(e => e.ExchangeRateUsed).HasPrecision(18, 6);

            b.HasIndex(e => e.ActivityId);

            b.HasOne<Activity>()
                .WithMany()
                .HasForeignKey(e => e.ActivityId)
                .OnDelete(DeleteBehavior.SetNull);

            b.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Expenses_OriginalAmount", "\"OriginalAmount\" >= 0");
                t.HasCheckConstraint("CK_Expenses_ConvertedAmount", "\"ConvertedAmount\" >= 0");
                t.HasCheckConstraint("CK_Expenses_ExchangeRateUsed", "\"ExchangeRateUsed\" > 0");
            });
        });

        // Flight
        builder.Entity<Flight>(b =>
        {
            b.HasIndex(f => f.TripId);
            b.Property(f => f.Airline).HasMaxLength(100).IsRequired();
            b.Property(f => f.FlightNumber).HasMaxLength(20).IsRequired();
            b.Property(f => f.DepartureAirport).HasMaxLength(10).IsRequired();
            b.Property(f => f.ArrivalAirport).HasMaxLength(10).IsRequired();
            b.Property(f => f.BookingReference).HasMaxLength(50);
            b.Property(f => f.Terminal).HasMaxLength(20);
            b.Property(f => f.Gate).HasMaxLength(20);
            b.Property(f => f.SeatNumber).HasMaxLength(10);
            b.Property(f => f.Notes).HasMaxLength(100);
        });
    }



}

