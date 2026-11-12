using System;
using MyTravel.Domain.Enums;

namespace MyTravel.Application.DTOs.Collaboration;

public sealed record TripMemberDto
{
    public Guid TripId { get; init; }
    public Guid UserId { get; init; }
    public string FullName { get; init; } = "";
    public string Email { get; init; } = "";
    public MemberRole Role { get; init; }

}
