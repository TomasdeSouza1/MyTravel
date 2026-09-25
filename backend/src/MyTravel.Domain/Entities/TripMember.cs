using MyTravel.Domain.Enums;

namespace MyTravel.Domain.Entities;

public class TripMember
{
    public Guid TripId { get; set; }
    public Guid UserId { get; set; }
    public MemberRole Role { get; set; } = MemberRole.Editor;

    public User? User { get; set; }
}
