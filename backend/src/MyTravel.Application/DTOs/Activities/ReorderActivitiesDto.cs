using System;

namespace MyTravel.Application.DTOs.Activities;

public sealed record ReorderActivitiesDto
{
    public List<ActivityOrderItemDto> Items { get; init; } = [];
}

public sealed record ActivityOrderItemDto
{
    public Guid ActivityId { get; init; }
    public int NewOrderIndex { get; init; }
}