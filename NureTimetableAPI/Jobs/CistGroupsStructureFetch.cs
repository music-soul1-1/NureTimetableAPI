using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Jobs;

public class CistGroupsStructureFetch(ICistRepository cistRepository, IPostgreSQLRepository postgreSQLRepository)
{
    private readonly ICistRepository _cistRepository = cistRepository;
    private readonly IPostgreSQLRepository _postgreSQLRepository = postgreSQLRepository;

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

        await _postgreSQLRepository.ClearAndSaveGroupsFacultiesAsync(groupsFaculties);
        Console.WriteLine("Saved groups faculties to DB successfully!");

        Console.ResetColor();

        return;
    }
}
