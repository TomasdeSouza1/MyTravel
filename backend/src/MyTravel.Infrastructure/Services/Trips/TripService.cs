using System;
using Microsoft.EntityFrameworkCore;                                                                                                                   
using MyTravel.Application.DTOs.Activities;                                                                                                            
using MyTravel.Application.DTOs.ItineraryDays;                                                                                                         
using MyTravel.Application.DTOs.Trips;                                                                                                                 
using MyTravel.Application.Interfaces.Trip;                                                                                                                 
using MyTravel.Domain.Entities;                                                                                                                        
using MyTravel.Domain.Enums;                                                                                                                           
using MyTravel.Infrastructure.Persistence;  




namespace MyTravel.Infrastructure.Services.Trips;

public class TripService(ApplicationDbContext context) : ITripService
{
    public async Task<TripResponseDto> CreateTripAsync(Guid userId, CreateTripDto dto)
    {
        var trip = new Trip                                                                                                                            
        {                                                                                                                                              
            UserId = userId,                                                                                                                           
            Title = dto.Title,                                                                                                                         
            Description = dto.Description,                                                                                                             
            DestinationCountry = dto.DestinationCountry,                                                                                               
            DestinationCity = dto.DestinationCity,                                                                                                     
            CoverImageUrl = dto.CoverImageUrl,                                                                                                         
            BaseCurrency = dto.BaseCurrency,                                                                                                           
            StartDate = dto.StartDate,                                                                                                                 
            EndDate = dto.EndDate,                                                                                                                     
            TotalBudget = dto.TotalBudget,                                                                                                             
            IsPublic = dto.IsPublic,                                                                                                                   
            InviteToken = Guid.NewGuid().ToString("N")                                                                                                 
        }; 
        //Generar dias automaticamente 
        var totalDays = dto.EndDate.DayNumber - dto.StartDate.DayNumber + 1;
        for (int i = 0; i < totalDays; i++)
        {
            trip.Days.Add(new ItineraryDay
            {
                DayNumber = i + 1,                                                                                                                     
                Date = dto.StartDate.AddDays(i),                                                                                                       
                LocationCountry = dto.DestinationCountry,                                                                                              
                LocationCity = dto.DestinationCity,                                                                                                    
                Activities = [] 
            });
        }
        trip.Members.Add(new TripMember
        {
            UserId = userId,
            Role = MemberRole.Owner
        });

        context.Trips.Add(trip);
        await context.SaveChangesAsync();
        return MapToResponseDto(trip);
    }
    public async Task<IEnumerable<TripResponseDto>> GetUserTripAsync(Guid userId)
    {
        return await context.Trips
            .AsNoTracking()
            .Where(t => t.UserId == userId || t.Members.Any(m => m.UserId == userId))
            .Select(t => new TripResponseDto
            {
                Id = t.Id,                                                                                                                             
                UserId = t.UserId,                                                                                                                     
                Title = t.Title,                                                                                                                       
                Description = t.Description,                                                                                                           
                DestinationCountry = t.DestinationCountry,                                                                                             
                DestinationCity = t.DestinationCity,                                                                                                   
                CoverImageUrl = t.CoverImageUrl,                                                                                                       
                BaseCurrency = t.BaseCurrency,                                                                                                         
                StartDate = t.StartDate,                                                                                                               
                EndDate = t.EndDate,                                                                                                                   
                TotalBudget = t.TotalBudget,                                                                                                           
                InviteToken = t.InviteToken,                                                                                                           
                IsPublic = t.IsPublic,                                                                                                                 
                CreatedAt = t.CreatedAt 
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<TripResponseDto>> GetPublicTripsAsync()
    {
        return await context.Trips
            .AsNoTracking()
            .Where(t => t.IsPublic)
            .OrderByDescending(t => t.CreatedAt)
            .Take(15)
            .Select(t => new TripResponseDto
            {
                Id = t.Id,                                                                                                                             
                UserId = t.UserId,                                                                                                                     
                Title = t.Title,                                                                                                                       
                Description = t.Description,                                                                                                           
                DestinationCountry = t.DestinationCountry,                                                                                             
                DestinationCity = t.DestinationCity,                                                                                                   
                CoverImageUrl = t.CoverImageUrl,                                                                                                       
                BaseCurrency = t.BaseCurrency,                                                                                                         
                StartDate = t.StartDate,                                                                                                               
                EndDate = t.EndDate,                                                                                                                   
                TotalBudget = t.TotalBudget,                                                                                                           
                // Seguridad: el InviteToken nunca se expone en feeds públicos anónimos.
                // Cualquiera con este token puede unirse como Editor al viaje ajeno.
                InviteToken = string.Empty,                                                                                                           
                IsPublic = t.IsPublic,                                                                                                                 
                CreatedAt = t.CreatedAt  
            })
            .ToListAsync();
    }

    public async Task<TripDetailResponseDto?> GetTripByIdAsync(Guid tripId, Guid? userId)
    {
         var trip = await context.Trips                                                                                                                 
                .AsNoTracking()                                                                                                                            
                .Include(t => t.Members)                                                                                                                   
                .Include(t => t.Days.OrderBy(d => d.DayNumber))                                                                                            
                    .ThenInclude(d => d.Activities.OrderBy(a => a.OrderIndex))                                                                             
                .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null) return null;
        
       var isOwnerOrMember = userId.HasValue && (trip.UserId == userId.Value || trip.Members.Any(m => m.UserId == userId.Value)); 
         
        //si el trip no es publico y el usuario no es owner o member, retornar null
        if (!trip.IsPublic && !isOwnerOrMember) return null;

        return new TripDetailResponseDto
        {
            Id = trip.Id,                                                                                                                              
                UserId = trip.UserId,                                                                                                                      
                Title = trip.Title,                                                                                                                        
                Description = trip.Description,                                                                                                            
                DestinationCountry = trip.DestinationCountry,                                                                                              
                DestinationCity = trip.DestinationCity,                                                                                                    
                CoverImageUrl = trip.CoverImageUrl,                                                                                                        
                BaseCurrency = trip.BaseCurrency,                                                                                                          
                StartDate = trip.StartDate,                                                                                                                
                EndDate = trip.EndDate,                                                                                                                    
                TotalBudget = trip.TotalBudget,                                                                                                            
                // Seguridad: solo el owner o un miembro puede ver el token de invitación.
                // Un visitante anónimo de un viaje público nunca debe recibirlo.
                InviteToken = isOwnerOrMember ? trip.InviteToken : string.Empty,                                                                      
                IsPublic = trip.IsPublic,                                                                                                                  
                CanEdit = isOwnerOrMember,                                                                                                                 
                CreatedAt = trip.CreatedAt,                                                                                                                
                Days = [.. trip.Days.Select(d => new ItineraryDayResponseDto                                                                                   
                {                                                                                                                                          
                    Id = d.Id,                                                                                                                             
                    TripId = d.TripId,                                                                                                                     
                    DayNumber = d.DayNumber,                                                                                                               
                    Date = d.Date,                                                                                                                         
                    LocationCountry = d.LocationCountry,                                                                                                   
                    LocationCity = d.LocationCity,                                                                                                         
                    WeatherSumm = d.WeatherSumm,                                                                                                           
                    TemperatureC = d.TemperatureC,                                                                                                         
                    Notes = d.Notes,                                                                                                                       
                    Activities = [.. d.Activities.Select(a => new ActivityResponseDto                                                                          
                    {                                                                                                                                      
                        Id = a.Id,                                                                                                                         
                        ItineraryDayId = a.ItineraryDayId,                                                                                                 
                        Name = a.Name,                                                                                                                     
                        Category = a.Category,                                                                                                             
                        Latitude = a.Latitude,                                                                                                             
                        Longitude = a.Longitude,                                                                                                           
                        Address = a.Address,                                                                                                               
                        StartTime = a.StartTime,                                                                                                           
                        EndTime = a.EndTime,                                                                                                               
                        OrderIndex = a.OrderIndex,                                                                                                         
                        BookingReference = a.BookingReference,                                                                                             
                        Notes = a.Notes                                                                                                                    
                    })]
                })]
        };             
        }

     public async Task<TripResponseDto?> UpdateTripAsync(Guid tripId, Guid userId, UpdateTripDto dto)
    {
        var trip = await context.Trips
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null) return null;

      var canEdit = trip.UserId == userId || trip.Members.Any(m => m.UserId == userId && (m.Role == MemberRole.Owner || m.Role == MemberRole.Editor));  

       if(!canEdit) return null;

        trip.Title = dto.Title;                                                                                                                        
        trip.Description = dto.Description;                                                                                                            
        trip.DestinationCountry = dto.DestinationCountry;                                                                                              
        trip.DestinationCity = dto.DestinationCity;                                                                                                    
        trip.CoverImageUrl = dto.CoverImageUrl;                                                                                                        
        trip.BaseCurrency = dto.BaseCurrency;                                                                                                          
        trip.StartDate = dto.StartDate;                                                                                                                
        trip.EndDate = dto.EndDate;                                                                                                                    
        trip.TotalBudget = dto.TotalBudget;                                                                                                            
        trip.IsPublic = dto.IsPublic; 

        await context.SaveChangesAsync();
        return MapToResponseDto(trip);
    }

    public async Task<bool> DeleteTripAsync(Guid tripId, Guid userId)
    {
        // Cargamos el viaje junto con todos sus hijos para propagar el soft delete.
        // ON DELETE CASCADE de PostgreSQL solo aplica a sentencias DELETE reales,
        // nunca a un UPDATE de IsDeleted = true.
        var trip = await context.Trips
            .Include(t => t.Days)
                .ThenInclude(d => d.Activities)
            .FirstOrDefaultAsync(t => t.Id == tripId);

        if (trip == null) return false;

        // Solo el Owner puede eliminar el viaje.
        if (trip.UserId != userId) return false;

        // Propagar soft delete a todos los descendientes.
        trip.IsDeleted = true;

        foreach (var day in trip.Days)
        {
            day.IsDeleted = true;
            foreach (var activity in day.Activities)
                activity.IsDeleted = true;
        }

        await context.SaveChangesAsync();
        return true;
    }

    private static TripResponseDto MapToResponseDto(Trip trip) => new()
    {
        Id = trip.Id,                                                                                                                                  
            UserId = trip.UserId,                                                                                                                          
            Title = trip.Title,                                                                                                                            
            Description = trip.Description,                                                                                                                
            DestinationCountry = trip.DestinationCountry,                                                                                                  
            DestinationCity = trip.DestinationCity,                                                                                                        
            CoverImageUrl = trip.CoverImageUrl,                                                                                                            
            BaseCurrency = trip.BaseCurrency,                                                                                                              
            StartDate = trip.StartDate,                                                                                                                    
            EndDate = trip.EndDate,                                                                                                                        
            TotalBudget = trip.TotalBudget,                                                                                                                
            InviteToken = trip.InviteToken,                                                                                                                
            IsPublic = trip.IsPublic,                                                                                                                      
            CreatedAt = trip.CreatedAt
    };



}




