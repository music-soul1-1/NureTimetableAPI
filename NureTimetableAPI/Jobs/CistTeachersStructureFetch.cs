using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Jobs;

public class CistTeachersStructureFetch(ICistRepository cistRepository, ISQLRepository postgreSQLRepository)
{
    private readonly ICistRepository _cistRepository = cistRepository;
    private readonly ISQLRepository _postgreSQLRepository = postgreSQLRepository;

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

        await _postgreSQLRepository.FetchTeachersFacultiesAsync(teachersFaculties);
        Console.WriteLine("Saved teachers faculties to DB successfully!");

        Console.ResetColor();
    }
}
