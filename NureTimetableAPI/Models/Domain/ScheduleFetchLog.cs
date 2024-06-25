using NureTimetableAPI.Types;

namespace NureTimetableAPI.Models.Domain;

public class ScheduleFetchLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime LastFetchedAt { get; set; }

    public EntityType Type { get; set; }

    public int EntityId { get; set; }
}