using MyTravel.Application.DTOs.Activities;

namespace MyTravel.Application.Interfaces.Activity;

public interface IActivityService
{
    Task<ActivityResponseDto?> AddActivityAsync(Guid dayId, Guid userId, CreateActivityDto dto);
    Task<ActivityResponseDto?> UpdateActivityAsync(Guid activityId, Guid userId, UpdateActivityDto dto);
    Task<bool> DeleteActivityAsync(Guid activityId, Guid userId);
    Task<bool> ReorderActivitiesAsync(Guid dayId, Guid userId, ReorderActivitiesDto dto);
}
