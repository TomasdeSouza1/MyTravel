using System;

namespace MyTravel.Application.DTOs.Activities;

public class ReorderActivitiesDto
{
    public List<ActivityOrderItemDto> Items{get;set;}=[];
}
public class ActivityOrderItemDto
{
    public Guid ActivityId {get; set;}
    public int NewOrderIndex{get; set;}
}