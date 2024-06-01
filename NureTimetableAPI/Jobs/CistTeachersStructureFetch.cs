using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Jobs;

public class CistTeachersStructureFetch(ICistRepository cistRepository, IPostgreSQLRepository postgreSQLRepository)
{
    private readonly ICistRepository _cistRepository = cistRepository;
    private readonly IPostgreSQLRepository _postgreSQLRepository = postgreSQLRepository;

    public async Task Execute()
    {
        await FetchTeachersFaculties();
    }

    public async Task FetchTeachersFaculties()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Fetching teachers faculties...");
        var teachersFaculties = await _cistRepository.GetTeachersFacultiesAsync();

        if (teachersFaculties == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to fetch teachers faculties");

            return;
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Fetched teachers faculties successfully!");

        await _postgreSQLRepository.ClearAndSaveTeachersFacultiesAsync(teachersFaculties);

        Console.ResetColor();
    }
}
