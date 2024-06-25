using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Jobs;

public class CistGroupsStructureFetch(ICistRepository cistRepository, ISQLRepository postgreSQLRepository)
{
    private readonly ICistRepository _cistRepository = cistRepository;
    private readonly ISQLRepository _postgreSQLRepository = postgreSQLRepository;

    public async Task Execute()
    {
        await FetchGroupsFaculties();
    }

    public async Task FetchGroupsFaculties()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Fetching groups faculties...");
        var groupsFaculties = await _cistRepository.GetGroupsFacultiesAsync();

        if (groupsFaculties == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to fetch groups faculties");

            return;
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Fetched groups faculties successfully!");

        await _postgreSQLRepository.FetchGroupsFacultiesAsync(groupsFaculties);
        Console.WriteLine("Saved groups faculties to DB successfully!");

        Console.ResetColor();

        return;
    }
}
