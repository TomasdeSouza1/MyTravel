using System;

namespace MyTravel.Application.DTOs.Collaboration;

public sealed record JointripDto
{
  public string InviteToken { get; init; } = ""; 
}
