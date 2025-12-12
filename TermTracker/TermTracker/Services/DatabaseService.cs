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
        var spring2025 = new Term
        {
            Name = "Spring 2025",
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 6, 30)
        };
        await _database.InsertAsync(spring2025);

        var mobileApp = new Course
        {
            Name = "Mobile Application Development",
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 3, 31),
            Status = StatusType.Active,
            InstructorName = "Dr. Sarah Mitchell",
            InstructorPhone = "555-0142",
            InstructorEmail = "sarah.mitchell@university.edu",
            TermId = spring2025.Id
        };
        await _database.InsertAsync(mobileApp);

        var mobileOA = new Assessment
        {
            Name = "Mobile Development Exam",
            CourseId = mobileApp.Id,
            StartDate = new DateTime(2025, 1, 15),
            EndDate = new DateTime(2025, 2, 15),
            Type = AssessmentType.Objective
        };
        await _database.InsertAsync(mobileOA);

        var mobilePA = new Assessment
        {
            Name = "Mobile App Project",
            CourseId = mobileApp.Id,
            StartDate = new DateTime(2025, 2, 16),
            EndDate = new DateTime(2025, 3, 30),
            Type = AssessmentType.Performance
        };
        await _database.InsertAsync(mobilePA);

        var mobileNote1 = new Note
        {
            Title = "Framework Comparison",
            Content = "Modern cross-platform frameworks offer unified project structure, performance enhancements, and better platform integration compared to older solutions.",
            CourseId = mobileApp.Id,
            Type = NoteType.large
        };
        await _database.InsertAsync(mobileNote1);

        var mobileNote2 = new Note
        {
            Title = "Course Resources",
            Content = "Online platform credentials needed. Weekly office hours: Thursdays 3PM EST. Review mobile design patterns and UI/UX best practices.",
            CourseId = mobileApp.Id,
            Type = NoteType.small
        };
        await _database.InsertAsync(mobileNote2);

        var dataStructures = new Course
        {
            Name = "Data Structures and Algorithms",
            StartDate = new DateTime(2025, 4, 1),
            EndDate = new DateTime(2025, 6, 30),
            Status = StatusType.Active,
            InstructorName = "Prof. James Rodriguez",
            InstructorPhone = "555-0198",
            InstructorEmail = "james.rodriguez@university.edu",
            TermId = spring2025.Id
        };
        await _database.InsertAsync(dataStructures);

        var dataStructuresOA = new Assessment
        {
            Name = "Algorithms Exam",
            CourseId = dataStructures.Id,
            StartDate = new DateTime(2025, 4, 15),
            EndDate = new DateTime(2025, 5, 15),
            Type = AssessmentType.Objective
        };
        await _database.InsertAsync(dataStructuresOA);

        var dataStructuresPA = new Assessment
        {
            Name = "Algorithm Implementation Project",
            CourseId = dataStructures.Id,
            StartDate = new DateTime(2025, 5, 16),
            EndDate = new DateTime(2025, 6, 25),
            Type = AssessmentType.Performance
        };
        await _database.InsertAsync(dataStructuresPA);

        var dataStructuresNote = new Note
        {
            Title = "Algorithm Complexity",
            Content = "Remember Big O notation: O(1) constant, O(log n) logarithmic, O(n) linear, O(n log n) linearithmic, O(n²) quadratic. Focus on optimization algorithms for the project.",
            CourseId = dataStructures.Id,
            Type = NoteType.large
        };
        await _database.InsertAsync(dataStructuresNote);

        var operatingSystems = new Course
        {
            Name = "Operating Systems",
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 3, 31),
            Status = StatusType.Active,
            InstructorName = "Dr. Kevin Park",
            InstructorPhone = "555-0401",
            InstructorEmail = "kevin.park@university.edu",
            TermId = spring2025.Id
        };
        await _database.InsertAsync(operatingSystems);

        var osOA = new Assessment
        {
            Name = "Operating Systems Exam",
            CourseId = operatingSystems.Id,
            StartDate = new DateTime(2025, 1, 20),
            EndDate = new DateTime(2025, 2, 20),
            Type = AssessmentType.Objective
        };
        await _database.InsertAsync(osOA);

        var osNote = new Note
        {
            Title = "Process vs Thread",
            Content = "Process: independent execution unit with own memory space. Thread: lightweight process sharing memory. Context switching between threads is faster than processes.",
            CourseId = operatingSystems.Id,
            Type = NoteType.small
        };
        await _database.InsertAsync(osNote);

        var fall2024 = new Term
        {
            Name = "Fall 2024",
            StartDate = new DateTime(2024, 7, 1),
            EndDate = new DateTime(2024, 12, 31)
        };
        await _database.InsertAsync(fall2024);

        var softwareI = new Course
        {
            Name = "Introduction to Software Development",
            StartDate = new DateTime(2024, 7, 1),
            EndDate = new DateTime(2024, 9, 30),
            Status = StatusType.Completed,
            InstructorName = "Dr. Emily Chen",
            InstructorPhone = "555-0223",
            InstructorEmail = "emily.chen@university.edu",
            TermId = fall2024.Id
        };
        await _database.InsertAsync(softwareI);

        var softwareIOA = new Assessment
        {
            Name = "Programming Fundamentals Exam",
            CourseId = softwareI.Id,
            StartDate = new DateTime(2024, 7, 15),
            EndDate = new DateTime(2024, 8, 15),
            Type = AssessmentType.Objective
        };
        await _database.InsertAsync(softwareIOA);

        var softwareIPA = new Assessment
        {
            Name = "Inventory Management System",
            CourseId = softwareI.Id,
            StartDate = new DateTime(2024, 8, 16),
            EndDate = new DateTime(2024, 9, 25),
            Type = AssessmentType.Performance
        };
        await _database.InsertAsync(softwareIPA);

        var softwareINote = new Note
        {
            Title = "GUI Framework Setup",
            Content = "Remember to configure the UI framework properly. Ensure all dependencies are correctly included in the project structure and build path.",
            CourseId = softwareI.Id,
            Type = NoteType.small
        };
        await _database.InsertAsync(softwareINote);

        var softwareII = new Course
        {
            Name = "Advanced Software Development",
            StartDate = new DateTime(2024, 10, 1),
            EndDate = new DateTime(2024, 12, 31),
            Status = StatusType.Completed,
            InstructorName = "Prof. Michael Thompson",
            InstructorPhone = "555-0267",
            InstructorEmail = "michael.thompson@university.edu",
            TermId = fall2024.Id
        };
        await _database.InsertAsync(softwareII);

        var softwareIIOA = new Assessment
        {
            Name = "Advanced Programming Exam",
            CourseId = softwareII.Id,
            StartDate = new DateTime(2024, 10, 15),
            EndDate = new DateTime(2024, 11, 15),
            Type = AssessmentType.Objective
        };
        await _database.InsertAsync(softwareIIOA);

        var softwareIIPA = new Assessment
        {
            Name = "Scheduling Application",
            CourseId = softwareII.Id,
            StartDate = new DateTime(2024, 11, 16),
            EndDate = new DateTime(2024, 12, 20),
            Type = AssessmentType.Performance
        };
        await _database.InsertAsync(softwareIIPA);

        var softwareIINote1 = new Note
        {
            Title = "Database Connection",
            Content = "Configure database connection strings properly. Remember to handle timezone conversions for date/time fields in the application.",
            CourseId = softwareII.Id,
            Type = NoteType.large
        };
        await _database.InsertAsync(softwareIINote1);

        var softwareIINote2 = new Note
        {
            Title = "Modern Language Features",
            Content = "Use modern language features like lambda expressions throughout the project. Good candidates: event handlers, stream operations, interface implementations.",
            CourseId = softwareII.Id,
            Type = NoteType.small
        };
        await _database.InsertAsync(softwareIINote2);

        var summer2025 = new Term
        {
            Name = "Summer 2025",
            StartDate = new DateTime(2025, 7, 1),
            EndDate = new DateTime(2025, 12, 31)
        };
        await _database.InsertAsync(summer2025);

        var capstone = new Course
        {
            Name = "Software Engineering Capstone",
            StartDate = new DateTime(2025, 7, 1),
            EndDate = new DateTime(2025, 9, 30),
            Status = StatusType.Active,
            InstructorName = "Dr. Patricia Williams",
            InstructorPhone = "555-0334",
            InstructorEmail = "patricia.williams@university.edu",
            TermId = summer2025.Id
        };
        await _database.InsertAsync(capstone);

        var capstoneOA = new Assessment
        {
            Name = "Project Proposal",
            CourseId = capstone.Id,
            StartDate = new DateTime(2025, 7, 15),
            EndDate = new DateTime(2025, 8, 1),
            Type = AssessmentType.Objective
        };
        await _database.InsertAsync(capstoneOA);

        var capstonePA = new Assessment
        {
            Name = "Final Project Implementation",
            CourseId = capstone.Id,
            StartDate = new DateTime(2025, 8, 2),
            EndDate = new DateTime(2025, 9, 25),
            Type = AssessmentType.Performance
        };
        await _database.InsertAsync(capstonePA);

        var capstoneNote = new Note
        {
            Title = "Project Requirements",
            Content = "Must demonstrate proficiency in software development lifecycle. Include documentation, testing, and deployment. Remember to highlight innovation and technical complexity.",
            CourseId = capstone.Id,
            Type = NoteType.large
        };
        await _database.InsertAsync(capstoneNote);

        var networkSecurity = new Course
        {
            Name = "Network Security Fundamentals",
            StartDate = new DateTime(2025, 10, 1),
            EndDate = new DateTime(2025, 12, 31),
            Status = StatusType.Active,
            InstructorName = "Prof. Lisa Martinez",
            InstructorPhone = "555-0512",
            InstructorEmail = "lisa.martinez@university.edu",
            TermId = summer2025.Id
        };
        await _database.InsertAsync(networkSecurity);

        var networkOA = new Assessment
        {
            Name = "Network Security Exam",
            CourseId = networkSecurity.Id,
            StartDate = new DateTime(2025, 10, 15),
            EndDate = new DateTime(2025, 11, 30),
            Type = AssessmentType.Objective
        };
        await _database.InsertAsync(networkOA);

        var networkNote1 = new Note
        {
            Title = "OSI Model Layers",
            Content = "7-Application, 6-Presentation, 5-Session, 4-Transport, 3-Network, 2-Data Link, 1-Physical. Mnemonic: All People Seem To Need Data Processing.",
            CourseId = networkSecurity.Id,
            Type = NoteType.small
        };
        await _database.InsertAsync(networkNote1);

        var networkNote2 = new Note
        {
            Title = "Common Ports",
            Content = "HTTP: 80, HTTPS: 443, FTP: 21, SSH: 22, Telnet: 23, SMTP: 25, DNS: 53, DHCP: 67/68, POP3: 110, IMAP: 143, RDP: 3389",
            CourseId = networkSecurity.Id,
            Type = NoteType.large
        };
        await _database.InsertAsync(networkNote2);

        var cloudComputing = new Course
        {
            Name = "Cloud Computing Essentials",
            StartDate = new DateTime(2025, 7, 1),
            EndDate = new DateTime(2025, 9, 30),
            Status = StatusType.Active,
            InstructorName = "Dr. Robert Chen",
            InstructorPhone = "555-0623",
            InstructorEmail = "robert.chen@university.edu",
            TermId = summer2025.Id
        };
        await _database.InsertAsync(cloudComputing);

        var cloudOA = new Assessment
        {
            Name = "Cloud Computing Exam",
            CourseId = cloudComputing.Id,
            StartDate = new DateTime(2025, 7, 20),
            EndDate = new DateTime(2025, 9, 15),
            Type = AssessmentType.Objective
        };
        await _database.InsertAsync(cloudOA);

        var cloudNote = new Note
        {
            Title = "Cloud Service Models",
            Content = "IaaS (Infrastructure as a Service): Virtual machines, storage. PaaS (Platform as a Service): Development frameworks. SaaS (Software as a Service): Complete applications.",
            CourseId = cloudComputing.Id,
            Type = NoteType.small
        };
        await _database.InsertAsync(cloudNote);
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

    public async Task<Course> GetCourseByIdAsync(int courseId)
    {
        await InitializeDatabaseAsync();

        var course = await _database.Table<Course>()
            .Where(c => c.Id == courseId)
            .FirstOrDefaultAsync();

        if (course != null)
        {
            course.Assessments = await _database.Table<Assessment>()
                .Where(a => a.CourseId == course.Id)
                .ToListAsync();

            course.Notes = await _database.Table<Note>()
                .Where(n => n.CourseId == course.Id)
                .ToListAsync();
        }

        return course;
    }

    public async Task AddCourseAsync(Course course)
    {
        await InitializeDatabaseAsync();
        await _database.InsertAsync(course);
    }

    public async Task UpdateCourseAsync(Course course)
    {
        await InitializeDatabaseAsync();
        await _database.UpdateAsync(course);
    }

    public async Task DeleteCourseAsync(int courseId)
    {
        await InitializeDatabaseAsync();

        var assessments = await _database.Table<Assessment>()
            .Where(a => a.CourseId == courseId)
            .ToListAsync();
        foreach (var assessment in assessments)
            await _database.DeleteAsync(assessment);

        var notes = await _database.Table<Note>()
            .Where(n => n.CourseId == courseId)
            .ToListAsync();
        foreach (var note in notes)
            await _database.DeleteAsync(note);

        var course = await _database.Table<Course>()
            .Where(c => c.Id == courseId)
            .FirstOrDefaultAsync();

        if (course != null)
        {
            await _database.DeleteAsync(course);
        }
    }

    public async Task AddAssessmentAsync(Assessment assessment)
    {
        await InitializeDatabaseAsync();
        await _database.InsertAsync(assessment);
    }

    public async Task UpdateAssessmentAsync(Assessment assessment)
    {
        await InitializeDatabaseAsync();
        await _database.UpdateAsync(assessment);
    }

    public async Task DeleteAssessmentAsync(int assessmentId)
    {
        await InitializeDatabaseAsync();
        var assessment = await _database.Table<Assessment>()
            .Where(a => a.Id == assessmentId)
            .FirstOrDefaultAsync();
        if (assessment != null)
        {
            await _database.DeleteAsync(assessment);
        }
    }

    public async Task AddNoteAsync(Note note)
    {
        await InitializeDatabaseAsync();
        await _database.InsertAsync(note);
    }

    public async Task UpdateNoteAsync(Note note)
    {
        await InitializeDatabaseAsync();
        await _database.UpdateAsync(note);
    }

    public async Task DeleteNoteAsync(int noteId)
    {
        await InitializeDatabaseAsync();
        var note = await _database.Table<Note>()
            .Where(n => n.Id == noteId)
            .FirstOrDefaultAsync();
        if (note != null)
        {
            await _database.DeleteAsync(note);
        }
    }

    public async Task ResetDatabaseAsync()
    {
        if (_database != null)
        {
            await _database.CloseAsync();
            _database = null;
        }

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "TermTracker.db");
        if (File.Exists(databasePath))
        {
            File.Delete(databasePath);
        }

        await InitializeDatabaseAsync();
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

