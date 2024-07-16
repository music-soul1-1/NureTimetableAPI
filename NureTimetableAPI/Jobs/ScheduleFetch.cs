using NureTimetableAPI.Repositories;
using NureTimetableAPI.Types;

namespace NureTimetableAPI.Jobs;

public class ScheduleFetch(ICistRepository cistRepository, ISQLRepository postgreSQLRepository)
{
    private readonly ISQLRepository repo = postgreSQLRepository;
    private readonly ICistRepository cist = cistRepository;

    public async Task Execute(int id, EntityType entityType)
    {
        var cistSchedule = await cist.GetCistScheduleAsync(id, entityType);

        if (cistSchedule != null)
        {
            await repo.FetchSchedule(id, cistSchedule, entityType);
        }
        // TODO: Log errors
    }
}
