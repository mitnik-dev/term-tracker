using SQLite;
using TermTracker.Models.Enums;


namespace TermTracker.Models;

public class Course
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [MaxLength(100), NotNull]
    public string Name { get; set; }
    [NotNull]
    public DateTime StartDate { get; set; }
    [NotNull]
    public DateTime EndDate { get; set; }
    [NotNull]
    public StatusType Status { get; set; }
    [MaxLength(100), NotNull]
    public string InstructorName { get; set; }
    [MaxLength(20), NotNull]
    public string InstructorPhone { get; set; }
    [MaxLength(100), NotNull]
    public string InstructorEmail { get; set; }
    [NotNull, Indexed]
    public int TermId { get; set; }
    [Ignore]
    public List<Assessment> Assessments { get; set; } = new();
    [Ignore]
    public List<Note> Notes { get; set; } = new();
}
