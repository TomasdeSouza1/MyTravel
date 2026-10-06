using MyTravel.Application.DTOs.ItineraryDays;

namespace MyTravel.Application.Interfaces.Itinerary;

public interface IItineraryDayService
{
    Task<ItineraryDayResponseDto?> AddDayAsync(Guid tripId, Guid userId, CreateItineraryDayDto dto);
    Task<ItineraryDayResponseDto?> UpdateDayAsync(Guid dayId, Guid userId, UpdateItineraryDayDto dto);
    Task<bool> DeleteDayAsync(Guid dayId, Guid userId);
}
