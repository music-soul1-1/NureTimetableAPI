using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Jobs;

public class CistBuildingsStructureFetch(ICistRepository cistRepository, ISQLRepository postgreRepository)
{
    private readonly ICistRepository _cistRepository = cistRepository;
    private readonly ISQLRepository _postgreRepository = postgreRepository;

    public async Task Execute()
    {
        await FetchAuditoriesBuildings();
    }

    public async Task FetchAuditoriesBuildings()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Fetching auditories buildings...");

        var buildings = await _cistRepository.GetBuildings();

        if (buildings == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to fetch auditories buildings");

            return;
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Fetched auditories buildings successfully!");

        await _postgreRepository.FetchBuildingsAsync(buildings);
        Console.WriteLine("Saved buildings to DB successfully!");

        Console.ResetColor();
    }
}
