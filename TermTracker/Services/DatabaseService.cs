using SQLite;
using TermTracker.Models;
using TermTracker.Models.Enums;

namespace TermTracker.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    public DatabaseService()
    {
    }


    private async Task SeedTestDataAsync()
    {
        var term1 = new Term
        {
            Name = "Spring 2025",
            StartDate = new DateTime(2025, 1, 6),
            EndDate = new DateTime(2025, 5, 15)
        };
        await _database.InsertAsync(term1);

        var term2 = new Term
        {
            Name = "Summer 2025",
            StartDate = new DateTime(2025, 5, 19),
            EndDate = new DateTime(2025, 8, 8)
        };
        await _database.InsertAsync(term2);

        var term3 = new Term
        {
            Name = "Fall 2025",
            StartDate = new DateTime(2025, 8, 25),
            EndDate = new DateTime(2025, 12, 19)
        };
        await _database.InsertAsync(term3);


        var course1 = new Course
        {
            Name = "Database Design",
            StartDate = new DateTime(2025, 1, 6),
            EndDate = new DateTime(2025, 3, 15),
            Status = StatusType.Completed,
            InstructorName = "Robert Johnson",
            InstructorPhone = "555-1234",
            InstructorEmail = "rjohnson@example.com",
            TermId = term1.Id
        };
        await _database.InsertAsync(course1);

        var course2 = new Course
        {
            Name = "Mobile Application Development",
            StartDate = new DateTime(2025, 3, 17),
            EndDate = new DateTime(2025, 5, 15),
            Status = StatusType.Active,
            InstructorName = "Sarah Williams",
            InstructorPhone = "555-5678",
            InstructorEmail = "swilliams@example.com",
            TermId = term1.Id
        };
        await _database.InsertAsync(course2);

        var course3 = new Course
        {
            Name = "Web Development",
            StartDate = new DateTime(2025, 5, 19),
            EndDate = new DateTime(2025, 7, 10),
            Status = StatusType.Future,
            InstructorName = "Michael Brown",
            InstructorPhone = "555-9012",
            InstructorEmail = "mbrown@example.com",
            TermId = term2.Id
        };
        await _database.InsertAsync(course3);

        var assessment1 = new Assessment
        {
            Name = "Database Design Project",
            CourseId = course1.Id,
            StartDate = new DateTime(2025, 2, 1),
            EndDate = new DateTime(2025, 3, 1),
            Type = AssessmentType.Objective
        };
        await _database.InsertAsync(assessment1);

        var assessment2 = new Assessment
        {
            Name = "SQL Query Assessment",
            CourseId = course1.Id,
            StartDate = new DateTime(2025, 2, 15),
            EndDate = new DateTime(2025, 3, 10),
            Type = AssessmentType.Performance
        };
        await _database.InsertAsync(assessment2);

        var assessment3 = new Assessment
        {
            Name = "Mobile App Project",
            CourseId = course2.Id,
            StartDate = DateTime.Now.AddDays(-10),
            EndDate = DateTime.Now.AddDays(20),
            Type = AssessmentType.Objective
        };
        await _database.InsertAsync(assessment3);

        var assessment4 = new Assessment
        {
            Name = "UI/UX Performance Test",
            CourseId = course2.Id,
            StartDate = DateTime.Now.AddDays(5),
            EndDate = DateTime.Now.AddDays(35),
            Type = AssessmentType.Performance
        };
        await _database.InsertAsync(assessment4);

        var note1 = new Note
        {
            Title = "Database Design Notes",
            Content = "Important concepts: ER diagrams, normalization, relationships. Review the textbook chapters 3-5.",
            CourseId = course1.Id,
            Type = NoteType.large
        };
        await _database.InsertAsync(note1);

        var note2 = new Note
        {
            Title = "Assignment Reminder",
            Content = "Complete the database design assignment by Friday. Review the ER diagram and ensure all relationships are properly defined.",
            CourseId = course1.Id,
            Type = NoteType.small
        };
        await _database.InsertAsync(note2);
    }

    public async Task<List<Term>> GetAllTermsAsync()
    {
        await InitializeDatabaseAsync();

        var terms = await _database.Table<Term>().ToListAsync();

        foreach (var term in terms)
        {
            term.Courses = await _database.Table<Course>()
                .Where(c => c.TermId == term.Id)
                .ToListAsync();

            foreach (var course in term.Courses)
            {
                course.Assessments = await _database.Table<Assessment>()
                    .Where(a => a.CourseId == course.Id)
                    .ToListAsync();

                course.Notes = await _database.Table<Note>()
                    .Where(n => n.CourseId == course.Id)
                    .ToListAsync();
            }
        }

        return terms;
    }

    public async Task<Term> GetTermByIdAsync(int termId)
    {
        await InitializeDatabaseAsync();

        var term = await _database.Table<Term>()
            .Where(t => t.Id == termId)
            .FirstOrDefaultAsync();

        if (term != null)
        {
            term.Courses = await _database.Table<Course>()
                .Where(c => c.TermId == term.Id)
                .ToListAsync();

            foreach (var course in term.Courses)
            {
                course.Assessments = await _database.Table<Assessment>()
                    .Where(a => a.CourseId == course.Id)
                    .ToListAsync();

                course.Notes = await _database.Table<Note>()
                    .Where(n => n.CourseId == course.Id)
                    .ToListAsync();
            }
        }

        return term;
    }

    public async Task<Term> GetCurrentTermAsync()
    {
        await InitializeDatabaseAsync();

        var today = DateTime.Today;
        var term = await _database.Table<Term>()
            .Where(t => t.StartDate <= today && t.EndDate >= today)
            .FirstOrDefaultAsync();

        if (term != null)
        {
            term.Courses = await _database.Table<Course>()
                .Where(c => c.TermId == term.Id)
                .ToListAsync();

            foreach (var course in term.Courses)
            {
                course.Assessments = await _database.Table<Assessment>()
                    .Where(a => a.CourseId == course.Id)
                    .ToListAsync();

                course.Notes = await _database.Table<Note>()
                    .Where(n => n.CourseId == course.Id)
                    .ToListAsync();
            }
        }

        return term;
    }

    public async Task<List<Assessment>> GetAllAssessmentsAsync()
    {
        await InitializeDatabaseAsync();
        return await _database.Table<Assessment>().ToListAsync();
    }

    public async Task AddTermAsync(Term term)
    {
        await InitializeDatabaseAsync();
        await _database.InsertAsync(term);
    }

    public async Task UpdateTermAsync(Term term)
    {
        await InitializeDatabaseAsync();
        await _database.UpdateAsync(term);
    }

    public async Task DeleteTermAsync(int termId)
    {
        await InitializeDatabaseAsync();

        var courses = await _database.Table<Course>()
            .Where(c => c.TermId == termId)
            .ToListAsync();

        foreach (var course in courses)
        {
            var assessments = await _database.Table<Assessment>()
                .Where(a => a.CourseId == course.Id)
                .ToListAsync();
            foreach (var assessment in assessments)
                await _database.DeleteAsync(assessment);

            var notes = await _database.Table<Note>()
                .Where(n => n.CourseId == course.Id)
                .ToListAsync();
            foreach (var note in notes)
                await _database.DeleteAsync(note);

            await _database.DeleteAsync(course);
        }

        var term = await _database.Table<Term>()
            .Where(t => t.Id == termId)
            .FirstOrDefaultAsync();

        if (term != null)
        {
            await _database.DeleteAsync(term);
        }
    }

    private async Task InitializeDatabaseAsync()
    {
        if (_database != null)
            return;

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "TermTracker.db");
        _database = new SQLiteAsyncConnection(databasePath);

        await _database.CreateTableAsync<Term>();
        await _database.CreateTableAsync<Course>();
        await _database.CreateTableAsync<Assessment>();
        await _database.CreateTableAsync<Note>();

        var termCount = await _database.Table<Term>().CountAsync();
        if (termCount == 0)
        {
            await SeedTestDataAsync();
        }
    }
}

