using NureTimetableAPI.Models.Cist;
using NureTimetableAPI.Repositories;
using NureTimetableAPI.Types;

namespace NureTimetableAPI.Jobs;

public class ScheduleFetch(ISQLRepository postgreSQLRepository)
{
    private readonly ISQLRepository repo = postgreSQLRepository;

    public async Task Execute(int id, CistSchedule cistSchedule, EntityType entityType)
    {
        await repo.FetchSchedule(id, cistSchedule, entityType);
    }
}
